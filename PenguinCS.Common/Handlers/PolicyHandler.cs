using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PenguinCS.Common.Interfaces;
using PenguinCS.Common.Responses;

namespace PenguinCS.Common.Handlers;

public class PolicyHandler(ILogger<PolicyHandler> logger) : IMessageHandler
{
    private readonly ILogger<PolicyHandler> _logger = logger;

    public async Task<IResponse> HandleMessageAsync(string messageContent, NetworkStream stream, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received policy file request");

        var responseMessage = "<cross-domain-policy><allow-access-from domain=\"*\" to-ports=\"*\" /></cross-domain-policy>";

        return new DisconnectResponse(responseMessage);
    }
}