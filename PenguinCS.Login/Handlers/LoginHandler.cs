using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PenguinCS.Common;
using PenguinCS.Common.Attributes;
using PenguinCS.Common.Extensions;
using PenguinCS.Common.Interfaces;
using PenguinCS.Common.Responses;
using PenguinCS.Data;
using StackExchange.Redis;

namespace PenguinCS.Login.Handlers;

[XMLMessageHandler("login")]
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

                return new DisconnectResponse(XTMessage.CreateError(150));
            }
        }

        #endregion

        #region Check Player Exists

        var player = _dbContext.Penguins.Where(p => p.Username == nickNode.InnerText.ToLower()).FirstOrDefault();

        if (player == null)
        {
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: '{nickname}' Not Found", stream.Socket.RemoteEndPoint, nickNode.InnerText);
            return new DisconnectResponse(XTMessage.CreateError(100));
        }
        #endregion

        #region Check Password

        bool passwordCorrect = BCrypt.Net.BCrypt.Verify(pwordNode.InnerText, player.Password);

        if (!passwordCorrect)
        {
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: Incorrect Password", stream.Socket.RemoteEndPoint);

            _ = IncrementFloodCounter(floodKey);
            return new DisconnectResponse(XTMessage.CreateError(101));
        }

        #endregion

        #region Activation

        var timeNow = new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified);
        // Needed for login data later
        var preactivationHours = 0;

        if (!player.Active)
        {
            var preactivation_expiry = player.RegistrationDate + TimeSpan.FromDays(_options.PreActivationDays);
            preactivationHours = (int)(preactivation_expiry - timeNow).TotalHours;

            if (timeNow > preactivation_expiry)
            {
                _logger.LogWarning("Client {RemoteEndPoint} failed to login: Account not activated", stream.Socket.RemoteEndPoint);
                return new DisconnectResponse(XTMessage.CreateError(900));
            }
        }

        #endregion

        #region Banned

        if (player.Permaban)
        {
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: Account banned", stream.Socket.RemoteEndPoint);
            return new DisconnectResponse(XTMessage.CreateError(603));
        }

        var activeBan = _dbContext.Bans.Where(b => b.PenguinId == player.Id && b.Expires > timeNow).OrderByDescending(b => b.Expires).FirstOrDefault();
        if (activeBan != null)
        {
            var hoursLeft = (activeBan.Expires - timeNow).TotalHours;
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: Account banned for {hoursLeft} hours", stream.Socket.RemoteEndPoint, hoursLeft);

            if (hoursLeft < 1)
                return new DisconnectResponse(XTMessage.CreateError(602));
            else
                return new DisconnectResponse(XTMessage.CreateError(601, hoursLeft.ToString()));
        }

        #endregion

        #region Parental Controls

        if (player.Grounded)
        {
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: Grounded", stream.Socket.RemoteEndPoint);
            return new DisconnectResponse(XTMessage.CreateError(913));
        }

        if (player.TimerActive)
        {
            if (!(player.TimerStart < TimeOnly.FromDateTime(timeNow) && player.TimerEnd > TimeOnly.FromDateTime(timeNow)))
            {
                _logger.LogWarning("Client {RemoteEndPoint} failed to login: Timer invalid", stream.Socket.RemoteEndPoint);
                return new DisconnectResponse(XTMessage.CreateError(911, player.TimerStart.ToString(), player.TimerEnd.ToString()));
            }

            if (TimeSpan.FromMinutes(GetMinutesPlayedToday(player.Id, _dbContext)) > player.TimerTotal)
            {
                _logger.LogWarning("Client {RemoteEndPoint} failed to login: Too many minutes played", stream.Socket.RemoteEndPoint);
                return new DisconnectResponse(XTMessage.CreateError(910, player.TimerTotal.ToString()));
            }
        }

        #endregion

        _logger.LogInformation("Client {remoteEndPoint} logged in", stream.Socket.RemoteEndPoint);

        var randomKey = Crypto.GenerateRandomKey();
        var loginKey = Crypto.Hash(randomKey);
        var confirmationHash = Crypto.Hash(Crypto.GenerateRandomKey(24));

        var loginPopulationTransaction = _connectionMultiplexer.GetDatabase().CreateTransaction();
        _ = loginPopulationTransaction.StringSetAsync($"{player.Username}.lkey", loginKey, TimeSpan.FromSeconds(_options.AuthTTLSeconds));
        _ = loginPopulationTransaction.StringSetAsync($"{player.Username}.ckey", confirmationHash, TimeSpan.FromSeconds(_options.AuthTTLSeconds));
        var redisPopulation = loginPopulationTransaction.HashGetAllAsync("houdini.population");

        // We can't get buddies until we have the population, so we have to split the transactions up.

        bool transactionSuccess = await loginPopulationTransaction.ExecuteAsync();
        if (!transactionSuccess)
        {
            _logger.LogError("Failed to set login key and confirmation hash for {username}, or failed to get populations", player.Username);
            // return new DisconnectResponse("%xt%e%-1%0%");
            return new DisconnectResponse(XTMessage.UnknownError);
        }

        var population = GetServerPopulation(redisPopulation.Result);

        var buddies = _dbContext.BuddyLists.Where(b => b.PenguinId == player.Id).Select(b => b.BuddyId).ToArray();

        var buddyTransaction = _connectionMultiplexer.GetDatabase().CreateTransaction();

        Dictionary<ushort, List<Task<bool>>> buddiesOnServerQuery = [];
        foreach (var server in population)
        {
            if (server.Value.Item2 <= 0)
                continue;

            List<Task<bool>> buddyQuery = [];

            foreach (var buddyId in buddies)
                buddyQuery.Add(buddyTransaction.SetContainsAsync($"houdini.players.{server.Key}", buddyId));

            buddiesOnServerQuery.Add(server.Key, buddyQuery);
        }

        transactionSuccess = await buddyTransaction.ExecuteAsync();
        if (!transactionSuccess)
        {
            _logger.LogError("Failed to query buddies for {username}", player.Username);
            // TODO: Should we really be kicking them for this, or just let them in with no buddy indicators?
            return new DisconnectResponse(XTMessage.UnknownError);
        }

        List<ushort> buddyOnServer = [];
        foreach (var server in buddiesOnServerQuery)
        {
            foreach (var buddy in server.Value)
            {
                if (buddy.Result == true)
                {
                    buddyOnServer.Add(server.Key);
                    break;
                }
            }
        }

        var approval = player.GetApproval();
        var rejection = player.GetRejection();

        List<string> populationList = [];
        foreach (var server in population)
            populationList.Add($"{server.Key},{server.Value.Item1}");
        var populationData = string.Join('|', populationList);

        var buddyData = string.Join('|', buddyOnServer);

        var rawLoginData = string.Join('|', player.Id, player.Id, player.Username, loginKey, _options.RandomKey, approval, rejection);
        var loginPacket = XTMessage.CreateMessage("l", rawLoginData, confirmationHash, string.Empty, populationData, buddyData, player.Email);
        if (!player.Active)
        {
            loginPacket += $"{preactivationHours}%";
        }

        return new RegularResponse(loginPacket);

    }

    private Dictionary<ushort, (ushort, ushort)> GetServerPopulation(HashEntry[] redisPopulation)
    {
        Dictionary<ushort, (ushort, ushort)> result = [];

        var redisDictionary = redisPopulation.ToDictionary();

        foreach (var kvp in redisDictionary)
        {
            var serverId = ushort.Parse(kvp.Key);
            var people = ushort.Parse(kvp.Value);
            ushort population = 0;

            if (people >= _options.MaxPlayers)
                population = 7;
            else
            {
                population = (ushort)Math.Floor(people / Math.Min(1, Math.Floor(_options.MaxPlayers / 6d)));

                if (population < 0 || population > 7)
                {
                    _logger.LogError("Server {serverId} is out of bounds: {population}", serverId, population);

                    throw new InvalidOperationException("Server population is out of bounds");
                }
            }

            result.Add(serverId, (population, people));
        }

        return result;
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
