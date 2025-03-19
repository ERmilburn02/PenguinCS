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
using StackExchange.Redis;

namespace PenguinCS.Login;

internal class LoginHostedService(ILogger<LoginHostedService> logger, IConnectionMultiplexer redis, MessageProcessor processor) : IHostedService
{
    private readonly ILogger<LoginHostedService> _logger = logger;
    private readonly IConnectionMultiplexer _redis = redis;
    private readonly MessageProcessor _processor = processor;
    private TcpListener _listener;
    private CancellationTokenSource _cancellationTokenSource;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        // TODO: Get from Config
        var port = 9912;

        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        _logger.LogInformation("Login Server started on port {port}", port);

        _ = AcceptClientAsync(_cancellationTokenSource.Token);

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Login Server...");

        _cancellationTokenSource.Cancel();
        _listener.Stop();

        await Task.Delay(500, CancellationToken.None); // Give a brief moment to complete ongoing tasks

        _logger.LogInformation("Login Server stopped.");
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

                var (messageFormat, idOrAction, extension) = GetMessageInfo(messageContent);

                switch (messageFormat)
                {
                    case EMessageFormat.XML:
                        await _processor.ProcessXMLMessageAsync(messageContent, idOrAction, stream, cancellationToken);
                        break;
                    case EMessageFormat.XT:
                        await _processor.ProcessXTMessageAsync(messageContent, idOrAction, extension, stream, cancellationToken);
                        break;
                    default:
                        throw new InvalidOperationException("Unknown message type");
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

    private (EMessageFormat format, string idOrAction, string extension) GetMessageInfo(string messageContent)
    {
        if (messageContent.StartsWith("<policy-file-request/>"))
        {
            return (EMessageFormat.XML, "policy-file-request", string.Empty);
        }
        else if (messageContent.StartsWith('<'))
        {
            // XML Packet, check the action attribute on the body
            XmlDocument xmlDoc = new();
            xmlDoc.LoadXml(messageContent);

            XmlElement rootElement = xmlDoc.DocumentElement;
            XmlNode bodyNode = rootElement.SelectSingleNode("body");
            string actionAttribute = ((XmlElement)bodyNode).GetAttribute("action");

            return (EMessageFormat.XML, actionAttribute, string.Empty);
        }
        else if (messageContent.StartsWith("%xt%"))
        {
            // XT Packet, get ext and id
            // TODO
            _logger.LogError("UNHANDLED XT PACKET: {messageContent}", messageContent);
            return (EMessageFormat.XT, "ERROR", "ERROR");
        }
        else
        {
            throw new InvalidOperationException("Unknown message type");
        }
    }
}
