using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PenguinCS.Common;

public class MessageProcessor(MessageHandlerRegistry registry, ILogger<MessageProcessor> logger)
{
    private readonly MessageHandlerRegistry _registry = registry;
    private readonly ILogger<MessageProcessor> _logger = logger;

    public async Task ProcessXTMessageAsync(string content, string id, string extension, NetworkStream stream, CancellationToken cancellationToken)
    {
        var handlers = _registry.GetXTHandlers(id, extension);

        if (handlers.Count == 0)
        {
            _logger.LogWarning("No XT Handler found for {id}#{extension}", id, extension);
            return;
        }

        foreach (var handler in handlers)
        {
            var response = await handler.HandleMessageAsync(content, stream, cancellationToken);
            await response.SendResponseAsync(stream, cancellationToken);
        }
    }

    public async Task ProcessXMLMessageAsync(string content, string action, NetworkStream stream, CancellationToken cancellationToken)
    {
        var handlers = _registry.GetXMLHandlers(action);

        if (handlers.Count == 0)
        {
            _logger.LogWarning("No XML Handler found for {action}", action);
            return;
        }

        foreach (var handler in handlers)
        {
            var response = await handler.HandleMessageAsync(content, stream, cancellationToken);
            await response.SendResponseAsync(stream, cancellationToken);
        }
    }
}