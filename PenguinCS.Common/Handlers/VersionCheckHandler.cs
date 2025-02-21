using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PenguinCS.Common.Interfaces;
using PenguinCS.Common.Responses;

namespace PenguinCS.Common.Handlers;

public class VersionCheckHandler(ILogger<VersionCheckHandler> logger, IOptions<PenguinCSOptions> options) : IMessageHandler
{
    private readonly ILogger<VersionCheckHandler> _logger = logger;
    private readonly PenguinCSOptions _options = options.Value;

    private const string SUCCESS_RESPONSE = "<msg t='sys'><body action='apiOK' r='0' /></msg>";
    private const string FAILURE_RESPONSE = "<msg t='sys'><body action='apiKO' r='0' /></msg>";

    public async Task<IResponse> HandleMessageAsync(string messageContent, NetworkStream stream, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received version check request");

        // Load the XML
        XmlDocument xmlDoc = new();
        xmlDoc.LoadXml(messageContent);

        // Get the version node
        XmlElement rootElement = xmlDoc.DocumentElement;
        XmlNode bodyNode = rootElement.SelectSingleNode("body");
        XmlNode verNode = bodyNode.SelectSingleNode("ver");

        // Get the version attribute
        string versionString = ((XmlElement)verNode).GetAttribute("v");
        ushort version = ushort.Parse(versionString);

        // TODO: Get version from config

        if (version == _options.VanillaVersion) // vanilla
        {
            _logger.LogInformation("Client is using vanilla client");

            return new RegularResponse(SUCCESS_RESPONSE);
        }
        else if (version == _options.LegacyVersion) // legacy 
        {
            _logger.LogWarning("Client is using legacy client");

            return new DisconnectResponse(FAILURE_RESPONSE);
        }
        else
        {
            _logger.LogWarning("Client is using an invalid version: {version}", version);

            return new DisconnectResponse(FAILURE_RESPONSE);
        }
    }
}
