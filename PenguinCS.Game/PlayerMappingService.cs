using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace PenguinCS.Game;

internal class PlayerMappingService(ILogger<PlayerMappingService> logger)
{
    private readonly ILogger<PlayerMappingService> _logger = logger;
    private readonly Dictionary<int, Player> Players = [];

    private readonly Dictionary<Socket, Player> PlayersBySocket = [];

    public void AddPlayer(Player player)
    {
        if (Players.ContainsKey(player.PID))
        {
            // TODO
        }

        Players.Add(player.PID, player);
        _logger.LogTrace("Added player {PID} to map", player.PID);

        if (PlayersBySocket.ContainsKey(player.Socket))
        {
            throw new InvalidOperationException("Player was added to Map multiple times");
        }

        PlayersBySocket.Add(player.Socket, player);
        _logger.LogTrace("Added player {PID}'s socket to map", player.PID);
    }

    public Player GetPlayer(int PID)
    {
        if (Players.TryGetValue(PID, out Player value))
        {
            return value;
        }

        return null;
    }

    public List<Player> GetAllPlayers()
    {
        return [.. Players.Values];
    }

    public Player GetPlayer(Socket socket)
    {
        if (PlayersBySocket.TryGetValue(socket, out Player value))
        {
            return value;
        }

        return null;
    }

    public void RemovePlayer(Socket socket)
    {
        if (PlayersBySocket.TryGetValue(socket, out Player player))
        {
            PlayersBySocket.Remove(socket);
            _logger.LogTrace("Removed player {PID}'s socket from map", player.PID);

            if (Players.ContainsKey(player.PID))
            {
                Players.Remove(player.PID);
                _logger.LogTrace("Removed player {PID} from map", player.PID);
            }
        }
    }
}