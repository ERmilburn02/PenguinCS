using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace PenguinCS.Common.Interfaces;

public interface IClientHandler
{
    Task AcceptClientAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken) => Task.CompletedTask;
}