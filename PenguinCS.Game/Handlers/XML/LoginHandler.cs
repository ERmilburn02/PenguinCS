using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PenguinCS.Common;
using PenguinCS.Common.Attributes;
using PenguinCS.Common.Interfaces;
using PenguinCS.Common.Responses;
using PenguinCS.Data;
using StackExchange.Redis;

namespace PenguinCS.Game.Handlers.XML;

[XMLMessageHandler("login")]
internal class LoginHandler(ILogger<LoginHandler> logger, ApplicationDbContext dbContext, IConnectionMultiplexer connectionMultiplexer, IOptions<PenguinCSOptions> options, PlayerMappingService playerMappingService) : IMessageHandler
{
    private readonly ILogger<LoginHandler> _logger = logger;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;
    private readonly PenguinCSOptions _options = options.Value;
    private readonly PlayerMappingService _playerMappingService = playerMappingService;
    
    // EXAMPLE
    // <msg t='sys'><body action='login' r='0'><login z='w1'><nick><![CDATA[101|101|basil|d41d8cd98f00b204e9800998ecf8427e|houdini|1|0]]></nick><pword><![CDATA[0c3b05a01028d3489d6440c1dab40e24d41d8cd98f00b204e9800998ecf8427e#95c2daac1c6583ad25d62db61c192f84]]></pword></login></body></msg>
    // nick: id|id|username|lkey|randomkey|approval|rejection
    // pword: client_key#confirmation_hash

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

        var nickData = nickNode.InnerText.Split('|');
        var pwordData = pwordNode.InnerText.Split('#');

        var userId = int.Parse(nickData[0]);
        var userIdCheck = int.Parse(nickData[1]);
        var nickName = nickData[2];
        var loginKey = nickData[3];
        var randomKey = nickData[4];
        var approval = int.Parse(nickData[5]);
        var rejection = int.Parse(nickData[6]);

        var clientKey = pwordData[0];
        var confirmationHash = pwordData[1];

        #region Safety Checks

        if (userId != userIdCheck)
        {
            _logger.LogWarning("Client {RemoteEndPoint} attempted to login with 2 different IDs ({userId} and {userIdCheck}), assuming bad client", stream.Socket.RemoteEndPoint, userId, userIdCheck);
            return new DisconnectResponse(XTMessage.UnknownError); // TODO: Check if there is a better error for "suspected modded client"
        }

        var player = _dbContext.Penguins.Where(p => p.Username == nickName.ToLower()).FirstOrDefault();

        if (player == null)
        {
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: '{nickname}' Not Found", stream.Socket.RemoteEndPoint, nickNode.InnerText);
            return new DisconnectResponse(XTMessage.CreateError(100));
        }

        if (player.Id != userId)
        {
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: ID {Id} does not match {userId}", stream.Socket.RemoteEndPoint, player.Id, userId);
            return new DisconnectResponse(XTMessage.UnknownError); // TODO: Check if there is a better error for "suspected modded client"
        }

        #endregion

        #region Redis

        var keyTransaction = redis.CreateTransaction();
        var loginKeyTask = keyTransaction.StringGetAsync($"{player.Username}.lkey");
        var confirmHashTask = keyTransaction.StringGetAsync($"{player.Username}.ckey");
        _ = keyTransaction.KeyDeleteAsync($"{player.Username}.lkey");
        _ = keyTransaction.KeyDeleteAsync($"{player.Username}.ckey");
        bool keyTransactionSuccess = await keyTransaction.ExecuteAsync();
        if (!keyTransactionSuccess)
        {
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: Key Transaction Failed!", stream.Socket.RemoteEndPoint);
            return new DisconnectResponse(XTMessage.UnknownError);
        }

        #endregion

        var loginHash = Crypto.EncryptPassword(loginKeyTask.Result.ToString() + _options.RandomKey) + loginKeyTask.Result.ToString();
        if (loginHash != clientKey)
        {
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: login hash ({loginHash}) does not match client key ({clientKey})", stream.Socket.RemoteEndPoint, loginHash, clientKey);
            return new DisconnectResponse(XTMessage.UnknownError);
        }

        if (loginKeyTask.Result.ToString() != loginKey)
        {
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: stored login key ({loginKeyTask}) does not match passed login key ({loginKey})", stream.Socket.RemoteEndPoint, loginKeyTask.Result.ToString(), loginKey);
            return new DisconnectResponse(XTMessage.UnknownError);
        }

        if (confirmHashTask.Result.ToString() != confirmationHash)
        {
            _logger.LogWarning("Client {RemoteEndPoint} failed to login: stored confirmation hash ({confirmHashTask}) does not match passed confirmation hash ({confirmationHash})", stream.Socket.RemoteEndPoint, confirmHashTask.Result.ToString(), confirmationHash);
            return new DisconnectResponse(XTMessage.UnknownError);
        }

        bool setLoginKeySuccess = await redis.StringSetAsync($"{player.Username}.loginkey", loginKey, TimeSpan.FromHours(12));
        if (!setLoginKeySuccess)
        {
            _logger.LogError("Failed to set login key!");
            return new DisconnectResponse(XTMessage.UnknownError);
        }

        // TODO: Check if server is above capacity and disconnect with 103 error

        // TODO: Check if Banned and disconnect

        var existingPlayer = _playerMappingService.GetPlayer(userId);
        if (existingPlayer != null)
        {
            _logger.LogWarning("{username} ({PID}) was already listed in the Player Mapping Service, disconnecting and removing them", player.Username, player.Id);
            existingPlayer.Disconnect();
            _playerMappingService.RemovePlayer(existingPlayer.Socket);
        }

        Player playerObj = new(userId, stream);
        playerObj.SetCachedPenguin(player);
        _playerMappingService.AddPlayer(playerObj);

        _logger.LogInformation("{username} logged in successfully from {RemoteEndPoint}", player.Username, stream.Socket.RemoteEndPoint);

        return new RegularResponse(XTMessage.CreateMessage("l"));
    }
}