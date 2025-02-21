using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace PenguinCS.Common.Interfaces;

public interface IResponse
{
    Task SendResponseAsync(NetworkStream stream, CancellationToken cancellationToken);
}