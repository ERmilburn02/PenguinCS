using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using PenguinCS.Common.Interfaces;
using PenguinCS.Common.Responses;
using PenguinCS.Data;
using StackExchange.Redis;

namespace PenguinCS.Login.Handlers;

internal class LoginHandler(ILogger<LoginHandler> logger, ApplicationDbContext dbContext, IConnectionMultiplexer connectionMultiplexer) : IMessageHandler
{
    private readonly ILogger<LoginHandler> _logger = logger;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;

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

            var player = _dbContext.Penguins.Where(p => p.Username == nickNode.InnerText.ToLower()).FirstOrDefault();

            if (player == null)
            {
                _logger.LogWarning("Client {RemoteEndPoint} failed to login: '{nickname}' Not Found", stream.Socket.RemoteEndPoint, nickNode.InnerText);
                return new DisconnectResponse("%xt%e%-1%100%");
            }

            bool passwordCorrect = BCrypt.Net.BCrypt.Verify(pwordNode.InnerText, player.Password);

            if (!passwordCorrect)
            {
                _logger.LogWarning("Client {RemoteEndPoint} failed to login: Incorrect Password", stream.Socket.RemoteEndPoint);

                _ = IncrementFloodCounter(floodKey);
                return new DisconnectResponse("%xt%e%-1%101%");
            }

            // TODO: temp
            return new DisconnectResponse("%xt%e%-1%150%");
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
