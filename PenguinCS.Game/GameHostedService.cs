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

namespace PenguinCS.Game;

internal class GameHostedService(
    ILogger<GameHostedService> logger, 
    IConnectionMultiplexer redis, 
    MessageProcessor processor, 
    PlayerMappingService playerMappingService
) : TcpService(logger, redis, processor)
{
    protected readonly PlayerMappingService PlayerMapping = playerMappingService;
    
    protected override string Name => "Game Server";
    protected override int Port => 9913;
    
    public override async Task AcceptClientAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var client = await Listener.AcceptTcpClientAsync(cancellationToken);
                _ = HandleClientAsync(client, cancellationToken);
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is InvalidOperationException)
            {
                break;
            }
        }
    }
    
    public override async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Client {RemoteEndPoint} connected.", client.Client.RemoteEndPoint);
        var stream = client.GetStream();
        
        try
        {
            while (client.Connected && !cancellationToken.IsCancellationRequested)
            {
                string messageContent = await stream.ReadUTF8NullTerminatedStringAsync(cancellationToken);
                
                if (string.IsNullOrWhiteSpace(messageContent))
                {
                    break;
                }
                
                Logger.LogTrace("Received from {RemoteEndPoint}: {message}", client.Client.RemoteEndPoint, messageContent);
                
                var (messageFormat, idOrAction, extension) = ResolveMessageInfo(messageContent);
                
                switch (messageFormat)
                {
                    case EMessageFormat.XML:
                        await Processor.ProcessXMLMessageAsync(messageContent, idOrAction, stream, cancellationToken);
                        break;
                    case EMessageFormat.XT:
                        await Processor.ProcessXTMessageAsync(messageContent, idOrAction, extension, stream, cancellationToken);
                        break;
                    default:
                        throw new InvalidOperationException("Unknown message type");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling client.");
        }
        finally
        {
            PlayerMapping.RemovePlayer(client.Client);
            client.Close();
            logger.LogInformation("Client disconnected.");
        }
    }
}