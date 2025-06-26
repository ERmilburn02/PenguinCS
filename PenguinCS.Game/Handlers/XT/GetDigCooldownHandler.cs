using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PenguinCS.Common;
using PenguinCS.Common.Attributes;
using PenguinCS.Common.Enums;
using PenguinCS.Common.Interfaces;
using PenguinCS.Common.Responses;
using StackExchange.Redis;

namespace PenguinCS.Game.Handlers.XT;

[XTMessageHandler("p", "getdigcooldown", EHandlerPolicy.Append)]
internal class GetDigCooldownHandler(ILogger<GetDigCooldownHandler> logger, IConnectionMultiplexer connectionMultiplexer, PlayerMappingService playerMappingService) :  IMessageHandler
{
    private readonly ILogger<GetDigCooldownHandler> _logger = logger;
    private readonly IConnectionMultiplexer _connectionMultiplexer =  connectionMultiplexer;
    private readonly PlayerMappingService _playerMappingService =  playerMappingService;
    
    public async Task<IResponse> HandleMessageAsync(string messageContent, NetworkStream stream, CancellationToken cancellationToken)
    {
        var player = _playerMappingService.GetPlayer(stream.Socket);

        if (player == null)
        {
            _logger.LogError("Player {SocketRemoteEndPoint} not found", stream.Socket.RemoteEndPoint);
            return new DisconnectResponse(XTMessage.UnknownError);
        }

        try
        {
            var redis = _connectionMultiplexer.GetDatabase();

            var digCooldownKey = $"houdini.last_dig.{player.PID}";


            if (!await redis.KeyExistsAsync(digCooldownKey))
            {
                _logger.LogTrace("Player {PID} has no dig cooldown", player.PID);
                return new RegularResponse(XTMessage.CreateMessage("getdigcooldown", 0.ToString()));
            }

            var lastDig = int.Parse(await redis.StringGetAsync(digCooldownKey));
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var cooldownRemaining = Math.Max(0, 120 - (currentTime - lastDig));
            _logger.LogTrace("Player {PID} has {cooldownRemaining} seconds dig cooldown remaining", player.PID,
                cooldownRemaining);

            return new RegularResponse(XTMessage.CreateMessage("getdigcooldown", cooldownRemaining.ToString()));
        }
        catch (RedisException ex)
        {
            _logger.LogCritical(ex, "Redis exception in GetDigCooldown");

            await player.SendMessageAsync(XTMessage.DatabaseError, cancellationToken);

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetDigCooldown");

            return new DisconnectResponse(XTMessage.UnknownError);
        }
    }
}