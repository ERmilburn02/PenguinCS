using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PenguinCS.Common.Interfaces;

namespace PenguinCS.Common.Responses;

public class NothingResponse : IResponse
{
    public Task SendResponseAsync(NetworkStream stream, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
