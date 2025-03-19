using System;
using Microsoft.Extensions.DependencyInjection;
using PenguinCS.Common;
using PenguinCS.Common.Handlers;
using PenguinCS.Common.Interfaces;
using PenguinCS.Game.Handlers.XML;

namespace PenguinCS.Game;

internal class MessageHandlerFactory(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IMessageHandler GetHandler(EMessageFormat format, string type)
    {
        return format switch
        {
            EMessageFormat.XML => GetXMLHandler(type),
            EMessageFormat.XT => GetXTHandler(type),

            _ => throw new NotImplementedException(),
        };
    }

    private IMessageHandler GetXMLHandler(string type)
    {
        return type switch
        {
            "policy-file-request" => _serviceProvider.GetRequiredService<PolicyHandler>(),

            "rndK" => _serviceProvider.GetRequiredService<RandomKeyHandler>(),
            "verChk" => _serviceProvider.GetRequiredService<VersionCheckHandler>(),

            "login" => _serviceProvider.GetRequiredService<LoginHandler>(),

            _ => throw new ArgumentException("Invalid message type", type)
        };
    }

    private IMessageHandler GetXTHandler(string type)
    {
        return type switch
        {
            _ => throw new ArgumentException("Invalid message type", type)
        };
    }
}