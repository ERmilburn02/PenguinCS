using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PenguinCS.Common;
using PenguinCS.Common.Interfaces;
using PenguinCS.Common.Responses;
using PenguinCS.Data;
using StackExchange.Redis;

namespace PenguinCS.Login.Handlers;

internal class LoginHandler(ILogger<LoginHandler> logger, ApplicationDbContext dbContext, IConnectionMultiplexer connectionMultiplexer, IOptions<PenguinCSOptions> options) : IMessageHandler
{
    private readonly ILogger<LoginHandler> _logger = logger;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;
    private readonly PenguinCSOptions _options = options.Value;

    public async Task<IResponse> HandleMessageAsync(string messageContent, NetworkStream stream, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received login request");

        var redis = _connectionMultiplexer.GetDatabase();

        // load the XML
        XmlDocument xmlDoc = new();
        xmlDoc.LoadXml(messageContent);

        XmlElement rootElement = xmlDoc.DocumentElement;
        XmlNode bodyNode = rootElement.SelectSingleNode("body");
        XmlNode loginNode = bodyNode.SelectSingleNode("login");
        XmlNode nickNode = loginNode.SelectSingleNode("nick");
        XmlNode pwordNode = loginNode.SelectSingleNode("pword");

        #region Flood

        string floodKey = string.Format("{0}.flood", stream.Socket.RemoteEndPoint.ToString()[..stream.Socket.RemoteEndPoint.ToString().IndexOf(':')]);


        if (await redis.KeyExistsAsync(floodKey))
        {
            var flood = ushort.Parse(await redis.StringGetAsync(floodKey));

            if (flood > 3) // Max Login Attempts
            {
                _logger.LogWarning("Client has exceeded login attempts");

                return new DisconnectResponse("%xt%e%-1%150%"); // TODO: make it easier to make XT packets
            }
        }

        #endregion

        #region Check Player Exists

        var player = _dbContext.Penguins.Where(p => p.Username == nickNode.InnerText.ToLower()).FirstOrDefault();

        if (player == null)
        {
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: '{nickname}' Not Found", stream.Socket.RemoteEndPoint, nickNode.InnerText);
            return new DisconnectResponse("%xt%e%-1%100%");
        }
        #endregion

        #region Check Password

        bool passwordCorrect = BCrypt.Net.BCrypt.Verify(pwordNode.InnerText, player.Password);

        if (!passwordCorrect)
        {
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: Incorrect Password", stream.Socket.RemoteEndPoint);

            _ = IncrementFloodCounter(floodKey);
            return new DisconnectResponse("%xt%e%-1%101%");
        }

        #endregion

        #region Activation

        var timeNow = new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified);

        if (!player.Active)
        {
            var preactivation_expiry = player.RegistrationDate + TimeSpan.FromDays(_options.PreActivationDays);

            if (timeNow > preactivation_expiry)
            {
                _logger.LogWarning("Client {RemoteEndPoint} failed to login: Account not activated", stream.Socket.RemoteEndPoint);
                return new DisconnectResponse("%xt%e%-1%900%");
            }
        }

        #endregion

        #region Banned

        if (player.Permaban)
        {
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: Account banned", stream.Socket.RemoteEndPoint);
            return new DisconnectResponse("%xt%e%-1%603%");
        }

        var activeBan = _dbContext.Bans.Where(b => b.PenguinId == player.Id && b.Expires > timeNow).OrderByDescending(b => b.Expires).FirstOrDefault();
        if (activeBan != null)
        {
            var hoursLeft = (activeBan.Expires - timeNow).TotalHours;
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: Account banned for {hoursLeft} hours", stream.Socket.RemoteEndPoint, hoursLeft);

            if (hoursLeft < 1)
                return new DisconnectResponse("%xt%e%-1%602%");
            else
                return new DisconnectResponse($"%xt%e%-1%601%{hoursLeft}%");
        }

        #endregion

        #region Parental Controls

        if (player.Grounded)
        {
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: Grounded", stream.Socket.RemoteEndPoint);
            return new DisconnectResponse("%xt%e%-1%913%");
        }

        if (player.TimerActive)
        {
            if (!(player.TimerStart < TimeOnly.FromDateTime(timeNow) && player.TimerEnd > TimeOnly.FromDateTime(timeNow)))
            {
                _logger.LogWarning("Client {RemoteEndPoint} failed to login: Timer invalid", stream.Socket.RemoteEndPoint);
                return new DisconnectResponse($"%xt%e%-1%911%{player.TimerStart.ToString()}%{player.TimerEnd.ToString()}%");
            }

            if (TimeSpan.FromMinutes(GetMinutesPlayedToday(player.Id, _dbContext)) > player.TimerTotal)
            {
                _logger.LogWarning("Client {RemoteEndPoint} failed to login: Too many minutes played", stream.Socket.RemoteEndPoint);
                return new DisconnectResponse($"%xt%e%-1%910%{player.TimerTotal}%");
            }
        }

        #endregion

        _logger.LogInformation("Client {remoteEndPoint} logged in", stream.Socket.RemoteEndPoint);

        // var randomKey = Crypto.GenerateRandomKey();
        // var loginKey = Crypto.Hash(randomKey);
        // var confirmationHash = Crypto.Hash(Crypto.GenerateRandomKey(24));

        // var redisTransaction = _connectionMultiplexer.GetDatabase().CreateTransaction();
        // _ = redisTransaction.StringSetAsync($"{player.Username}.lkey", loginKey, TimeSpan.FromSeconds(_options.AuthTTLSeconds));
        // _ = redisTransaction.StringSetAsync($"{player.Username}.ckey", confirmationHash, TimeSpan.FromSeconds(_options.AuthTTLSeconds));
        // // TODO: Buddy and Population checks (add to transaction)
        // bool transactionSuccess = await redisTransaction.ExecuteAsync();
        // if (!transactionSuccess)
        // {
        //     _logger.LogError("Failed to set login key and confirmation hash for {username}", player.Username);
        //     return new DisconnectResponse("%xt%e%-1%0%");
        // }

        // TODO: temp
        return new DisconnectResponse($"%xt%e%-1%910%{TimeSpan.FromHours(6) + TimeSpan.FromSeconds(9)}%");
        // return new DisconnectResponse("%xt%e%-1%150%");

    }

    private static long GetMinutesPlayedToday(int id, ApplicationDbContext dbContext)
    {
        return dbContext.Logins.Where(l => l.PenguinId == id && l.Date.Date == DateTime.UtcNow.Date).Sum(l => l.MinutesPlayed);
    }

    private async Task IncrementFloodCounter(string floodKey)
    {
        var redis = _connectionMultiplexer.GetDatabase();

        var trans = redis.CreateTransaction();
        var flood = trans.StringIncrementAsync(floodKey);
        _ = trans.KeyExpireAsync(floodKey, TimeSpan.FromSeconds(60 * 60));
        var expiry = trans.KeyTimeToLiveAsync(floodKey);
        bool success = await trans.ExecuteAsync();

        if (!success)
        {
            throw new Exception($"Incrementing flood counter failed for {floodKey}");
        }

        _logger.LogInformation("Incremented {floodKey} to {flood}, expires in {Expiry}", floodKey, flood.Result, expiry.Result);
    }
}
