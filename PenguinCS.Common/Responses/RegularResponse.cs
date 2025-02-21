using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PenguinCS.Common.Extensions;
using PenguinCS.Common.Interfaces;

namespace PenguinCS.Common.Responses;

public class RegularResponse(string message) : IResponse
{
    public string Message { get; } = message;

    public async Task SendResponseAsync(NetworkStream stream, CancellationToken cancellationToken)
    {
        await stream.WriteUTF8NullTerminatedStringAsync(Message, cancellationToken);
    }
}