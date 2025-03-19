using System;
using Microsoft.Extensions.DependencyInjection;
using PenguinCS.Common;
using PenguinCS.Common.Enums;
using PenguinCS.Common.Handlers;
using PenguinCS.Common.Interfaces;
using PenguinCS.Login.Handlers;

namespace PenguinCS.Login;

internal class MessageHandlerFactory(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IMessageHandler GetHandler(EMessageFormat format, string type)
    {
        if (format != EMessageFormat.XML)
        {
            throw new ArgumentException("Login Messages must be XML Format!");
        }

        return type switch
        {
            "policy-file-request" => _serviceProvider.GetRequiredService<PolicyHandler>(),

            "rndK" => _serviceProvider.GetRequiredService<RandomKeyHandler>(),
            "verChk" => _serviceProvider.GetRequiredService<VersionCheckHandler>(),
            "login" => _serviceProvider.GetRequiredService<LoginHandler>(),

            _ => throw new ArgumentException("Invalid message type", type)
        };
    }
}