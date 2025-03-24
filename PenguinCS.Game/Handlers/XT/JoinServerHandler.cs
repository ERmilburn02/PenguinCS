using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PenguinCS.Common;
using PenguinCS.Common.Attributes;
using PenguinCS.Common.Enums;
using PenguinCS.Common.Extensions;
using PenguinCS.Common.Interfaces;
using PenguinCS.Common.Responses;
using StackExchange.Redis;

namespace PenguinCS.Game.Handlers.XT;

[XTMessageHandler("j", "js", EHandlerPolicy.Append)]
internal class JoinServerHandler(ILogger<JoinServerHandler> logger, PlayerMappingService playerMappingService, IConnectionMultiplexer connectionMultiplexer) : IMessageHandler
{
    private readonly ILogger<JoinServerHandler> _logger = logger;
    private readonly PlayerMappingService _playerMappingService = playerMappingService;
    private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;

    // EXAMPLE
    // %xt%s%j#js%-1%101%d41d8cd98f00b204e9800998ecf8427e%en%
    // PID % LOGIN_KEY % LANG (ignored in houdini)

    public async Task<IResponse> HandleMessageAsync(string messageContent, NetworkStream stream, CancellationToken cancellationToken)
    {
        var messageParts = messageContent.Split('%');
        // EMPTY, xt, s, j#js, -1, PID, LoginKey, Lang
        var providedPID = int.Parse(messageParts[5]);
        var providedLoginKey = messageParts[6];

        Player player = _playerMappingService.GetPlayer(stream.Socket);
        if (player == null || player.PID != providedPID)
        {
            _logger.LogWarning("{RemoteEndPoint} attempted to join without a valid PID", stream.Socket.RemoteEndPoint);
            return new DisconnectResponse(XTMessage.UnknownError);
        }

        var redis = _connectionMultiplexer.GetDatabase();
        var loginKeyRedis = await redis.StringGetAsync($"{player.Username}.loginkey");

        if (loginKeyRedis != providedLoginKey)
        {
            _logger.LogWarning("{Username} ({PID}) attempted to join without a valid Login Key", player.Username, player.PID);
            return new DisconnectResponse(XTMessage.UnknownError);
        }

        await player.SendMessageAsync(XTMessage.CreateMessage("activefeatures"), cancellationToken);

        int moderatorStatus = 0;
        if (player.CachedPenguin.Character != null)
            moderatorStatus = 3;
        else if (player.CachedPenguin.StealthModerator)
            moderatorStatus = 2;
        else if (player.CachedPenguin.Moderator)
            moderatorStatus = 1;

        await player.SendMessageAsync(XTMessage.CreateMessage("js", player.CachedPenguin.AgentStatus ? "1" : "0", 0.ToString(), moderatorStatus.ToString(), player.CachedPenguin.BookModified.ToString()), cancellationToken);

        var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var penguinStandardTime = currentTime * 1000;
        var serverTimeZone = "GMT"; // TODO: Replace with config
        var tz = TimeZoneInfo.FindSystemTimeZoneById(serverTimeZone);
        DateTimeOffset dt = TimeZoneInfo.ConvertTime(DateTimeOffset.FromUnixTimeSeconds(currentTime), tz);
        var serverTimeOffset = Math.Abs(dt.Offset.Hours);

        // _logger.LogInformation("Current Time: {currentTime}\n" +
        //                        "Penguin Standard Time: {penguinStandardTime}\n" +
        //                        "Server Time Offset: {serverTimeOffset}", currentTime, penguinStandardTime, serverTimeOffset);

        // TODO: Egg Timer code
        var eggTimerMinutes = 24 * 60;

        await player.SendMessageAsync(XTMessage.CreateMessage("lp", /* TODO: STRING COMPILED PENGUIN */"", player.CachedPenguin.Coins.ToString(),
                                                              player.CachedPenguin.SafeChat ? "1" : "0", eggTimerMinutes.ToString(), penguinStandardTime.ToString(),
                                                              player.CachedPenguin.GetAgeInDays().ToString(), 0.ToString(), player.CachedPenguin.MinutesPlayed.ToString(),
                                                              /* Membership Days Remain */100.ToString(), serverTimeOffset.ToString(),
                                                              player.CachedPenguin.OpenedPlayercard ? "1" : "0", player.CachedPenguin.MapCategory.ToString(),
                                                              player.CachedPenguin.StatusField.ToString()), cancellationToken);

        // TODO: Pick and join spawn room

        player.SetHasJoined(true);

        var serverId = 9913; // TODO: Move to config
        var serverKey = $"houdini.players.{serverId}";

        var updateTransaction = redis.CreateTransaction();
        var setTask = updateTransaction.SetAddAsync(serverKey, player.PID);
        var hashTask = updateTransaction.HashSetAsync("houdini.population", [new HashEntry(serverId, _playerMappingService.GetAllPlayers().Where(p => p.HasJoined == true).Count())]);
        await updateTransaction.ExecuteAsync();

        if (!setTask.IsCompletedSuccessfully)
        {
            _logger.LogWarning("An error occurred while trying to add {Username} ({PID}) to the Redis set", player.Username, player.PID);
        }

        if (!hashTask.IsCompletedSuccessfully)
        {
            _logger.LogWarning("An error occurred while trying to set the population hash for server {serverId}", serverId);
        }

        // TEMP
        return new NothingResponse();
    }
}