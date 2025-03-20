using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PenguinCS.Common.Extensions;
using PenguinCS.Data;

namespace PenguinCS.Game;

internal class Player(int pid, NetworkStream stream)
{
    public int PID { get; } = pid;
    public Socket Socket => Stream.Socket;
    public NetworkStream Stream { get; } = stream;
    public Penguin CachedPenguin { get; private set; }

    public bool IsConnected => Socket.Connected;
    public void Disconnect() => Stream.Close();

    public void SetPenguin(Penguin penguin)
    {
        CachedPenguin = penguin;
    }

    public void SendMessage(string message)
    {
        Stream.WriteUTF8NullTerminatedString(message);
    }

    public async Task SendMessageAsync(string message, CancellationToken cancellationToken)
    {
        await Stream.WriteUTF8NullTerminatedStringAsync(message, cancellationToken);
    }
}