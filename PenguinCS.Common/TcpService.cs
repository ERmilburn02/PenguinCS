using System;
using System.Net;
using System.Xml;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PenguinCS.Common.Interfaces;
using PenguinCS.Common.Enums;
using StackExchange.Redis;

namespace PenguinCS.Common;

public class TcpService(
    ILogger<TcpService> logger,
    IConnectionMultiplexer redis,
    MessageProcessor processor
) : IClientHandler, IHostedService
{
    private CancellationTokenSource cancellationTokenSource;
    protected readonly MessageProcessor Processor = processor;
    protected readonly IConnectionMultiplexer Redis = redis;
    protected readonly ILogger Logger = logger;
    protected TcpListener Listener;
    
    protected virtual string Name => "Server";
    protected virtual int Port => 8000;
    
    public virtual async Task AcceptClientAsync(CancellationToken cancellationToken)
    {
    }
    
    public virtual async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        Listener = new TcpListener(IPAddress.Any, Port);
        Listener.Start();
        Logger.LogInformation("{name} started on port {port}", Name, Port);

        _ = AcceptClientAsync(cancellationTokenSource.Token);

        return Task.CompletedTask;
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Stopping {name}...", Name);

        await cancellationTokenSource.CancelAsync();
        Listener.Stop();
        Logger.LogInformation("{name} stopped.", Name);
    }

    protected (EMessageFormat format, string idOrAction, string extension) ResolveMessageInfo(string messageContent)
    {
        if (messageContent.StartsWith("<policy-file-request/>"))
        {
            // We received a flash socket policy request, which serves a similar purpose to CORS
            // nowadays. Usually, there is an external policy server running on port 843; however
            // once that one is not available this is used as a backup.
            return (EMessageFormat.XML, "policy-file-request", string.Empty);
        }
        
        // XML Packets
        if (messageContent.StartsWith('<'))
        {
            XmlDocument xml = new();
            xml.LoadXml(messageContent);
            
            XmlElement rootElement = xml.DocumentElement;
            
            if (rootElement == null)
                throw new InvalidOperationException($"No root element found for {messageContent}");
            
            // Check the action attribute on the body
            XmlNode bodyNode = rootElement.SelectSingleNode("body");
            string actionAttribute = ((XmlElement)bodyNode)?.GetAttribute("action");
            
            return (EMessageFormat.XML, actionAttribute, string.Empty);
        }
        
        // XT Packets
        if (messageContent.StartsWith("%xt%"))
        {
            // Example: %xt%s%j#js%-1%101%d41d8cd98f00b204e9800998ecf8427e%en%
            
            // Message Parts:
            // 1: Blank
            // 2: xt
            // 3: s
            // 4: id#ext (e.g. j#js)
            
            // Parse message parts
            var xtParts = messageContent.Split('%');
            
            // Parse 'id' & 'ext'
            var xtData = xtParts[3].Split('#');
            var id = xtData[0];
            var extension = xtData[1];
            
            return (EMessageFormat.XT, id, extension);
        }
        
        throw new InvalidOperationException("Unknown message type");
    }
}