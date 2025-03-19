using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PenguinCS.Common;
using PenguinCS.Common.Enums;
using PenguinCS.Common.Extensions;
using PenguinCS.Common.Responses;
using PenguinCS.Data;
using StackExchange.Redis;

namespace PenguinCS.Game;

internal class GameHostedService(ILogger<GameHostedService> logger, IConnectionMultiplexer redis, ApplicationDbContext dbContext, MessageHandlerFactory messageHandlerFactory) : IHostedService
{
    private readonly ILogger<GameHostedService> _logger = logger;
    private readonly IConnectionMultiplexer _redis = redis;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly MessageHandlerFactory _messageHandlerFactory = messageHandlerFactory;
    private TcpListener _listener;
    private CancellationTokenSource _cancellationTokenSource;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        var port = 9913;

        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        _logger.LogInformation("Game Server started on port {port}", port);

        _ = AcceptClientAsync(_cancellationTokenSource.Token);

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Game Server...");

        _cancellationTokenSource.Cancel();
        _listener.Stop();

        await Task.Delay(500, CancellationToken.None); // Give a brief moment to complete ongoing tasks

        _logger.LogInformation("Game Server stopped.");
    }

    private async Task AcceptClientAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var client = await _listener.AcceptTcpClientAsync(cancellationToken);
                _ = HandleClientAsync(client, cancellationToken);
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is InvalidOperationException)
            {
                break;
            }
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Client {RemoteEndPoint} connected.", client.Client.RemoteEndPoint);
        var stream = client.GetStream();

        try
        {
            var redis = _redis.GetDatabase();

            while (client.Connected && !cancellationToken.IsCancellationRequested)
            {
                string messageContent = stream.ReadUTF8NullTerminatedString();

                if (string.IsNullOrWhiteSpace(messageContent))
                {
                    break;
                }

                _logger.LogTrace("Received from {RemoteEndPoint}: {message}", client.Client.RemoteEndPoint, messageContent);

                var (messageFormat, messageType) = GetMessageInfo(messageContent);
                var handler = _messageHandlerFactory.GetHandler(messageFormat, messageType);
                var response = await handler.HandleMessageAsync(messageContent, stream, cancellationToken);

                await response.SendResponseAsync(stream, cancellationToken);

                if (response is DisconnectResponse)
                {
                    _logger.LogInformation("Disconnecting client as per request.");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling client.");
        }
        finally
        {
            client.Close();
            _logger.LogInformation("Client disconnected.");
        }
    }

    private (EMessageFormat, string) GetMessageInfo(string messageContent)
    {
        if (messageContent.StartsWith("<policy-file-request/>"))
        {
            return (EMessageFormat.XML, "policy-file-request");
        }
        else if (messageContent.StartsWith('<'))
        {
            // XML Packet, check the action attribute on the body
            XmlDocument xmlDoc = new();
            xmlDoc.LoadXml(messageContent);

            XmlElement rootElement = xmlDoc.DocumentElement;
            XmlNode bodyNode = rootElement.SelectSingleNode("body");
            string actionAttribute = ((XmlElement)bodyNode).GetAttribute("action");

            return (EMessageFormat.XML, actionAttribute);
        }
        else if (messageContent.StartsWith("%xt%"))
        {
            // XT Packet, get ext and id
            // TODO
            _logger.LogError("UNHANDLED XT PACKET: {messageContent}", messageContent);
            return (EMessageFormat.XT, "ERROR");
        }
        else
        {
            throw new InvalidOperationException("Unknown message type");
        }
    }
}