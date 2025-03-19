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
internal class LoginHandler(ILogger<LoginHandler> logger, ApplicationDbContext dbContext, IConnectionMultiplexer connectionMultiplexer, IOptions<PenguinCSOptions> options) : IMessageHandler
{
    private readonly ILogger<LoginHandler> _logger = logger;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;
    private readonly PenguinCSOptions _options = options.Value;
    
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

        return new DisconnectResponse(XTMessage.UnknownError);
    }
}