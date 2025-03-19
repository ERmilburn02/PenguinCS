using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PenguinCS.Common.Extensions;
using PenguinCS.Common.Interfaces;

namespace PenguinCS.Common.Responses;

public class DisconnectResponse(string message) : IResponse
{
    public string Message { get; } = message;

    public async Task SendResponseAsync(NetworkStream stream, CancellationToken cancellationToken)
    {
        await stream.WriteUTF8NullTerminatedStringAsync(Message, cancellationToken);

        stream.Close(); // Close Stream after sending response
    }
}