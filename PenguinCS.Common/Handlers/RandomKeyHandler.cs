using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PenguinCS.Common.Attributes;
using PenguinCS.Common.Interfaces;
using PenguinCS.Common.Responses;

namespace PenguinCS.Common.Handlers;

[XMLMessageHandler("rndK")]
public class RandomKeyHandler(ILogger<RandomKeyHandler> logger, IOptions<PenguinCSOptions> options) : IMessageHandler
{
    private readonly ILogger<RandomKeyHandler> _logger = logger;
    private readonly PenguinCSOptions _options = options.Value;

    public async Task<IResponse> HandleMessageAsync(string messageContent, NetworkStream stream, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received random key request");

        // TODO: Get key from config
        
        var key = _options.RandomKey ?? "houdini";
        var responseMessage = string.Format("<msg t='sys'><body action='rndK' r='-1'><k>{0}</k></body></msg>", key);

        return new RegularResponse(responseMessage);
    }
}