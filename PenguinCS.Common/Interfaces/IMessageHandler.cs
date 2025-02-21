using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace PenguinCS.Common.Interfaces;

public interface IMessageHandler
{
    Task<IResponse> HandleMessageAsync(string messageContent, NetworkStream stream, CancellationToken cancellationToken);
}