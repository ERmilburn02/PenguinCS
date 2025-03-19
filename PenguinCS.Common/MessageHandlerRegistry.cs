using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PenguinCS.Common.Attributes;
using PenguinCS.Common.Enums;
using PenguinCS.Common.Interfaces;

namespace PenguinCS.Common;

public class MessageHandlerRegistry
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MessageHandlerRegistry> _logger;
    private readonly Dictionary<(string Id, string Extension), List<Type>> _xtHandlerMappings = [];
    private readonly Dictionary<string, List<Type>> _xmlHandlerMappings = [];

    public MessageHandlerRegistry(IServiceProvider serviceProvider, ILogger<MessageHandlerRegistry> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        RegisterInternalHandlers();
    }

    private void RegisterInternalHandlers()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.StartsWith("PenguinCS"));
        var handlerTypes = GetHandlerList(assemblies);

        foreach (var handlerType in handlerTypes)
        {
            _logger.LogTrace("Checking attributes for {handlerType}", handlerType);
            // Check for XT Attribute
            var xtAttribute = handlerType.GetCustomAttribute<XTMessageHandlerAttribute>();
            if (xtAttribute != null)
            {
                _logger.LogTrace("Type {handlerType} is an XT Handler", handlerType);
                RegisterXTHandler(xtAttribute, handlerType);
                continue;
            }

            var xmlAttribute = handlerType.GetCustomAttribute<XMLMessageHandlerAttribute>();
            if (xmlAttribute != null)
            {
                _logger.LogTrace("Type {handlerType} is an XML Handler", handlerType);
                RegisterXMLHandler(xmlAttribute, handlerType);
                continue;
            }

            _logger.LogWarning("Type {handlerType} has no recognised attributes!", handlerType);
        }
    }

    private void RegisterXTHandler(XTMessageHandlerAttribute attribute, Type handlerType)
    {
        var key = (attribute.Id, attribute.Extension);

        if (!_xtHandlerMappings.ContainsKey(key))
        {
            _xtHandlerMappings[key] = [];
        }

        if (attribute.Policy == EHandlerPolicy.Overwrite)
        {
            _xtHandlerMappings[key].Clear();
        }

        _xtHandlerMappings[key].Add(handlerType);
    }

    private void RegisterXMLHandler(XMLMessageHandlerAttribute attribute, Type handlerType)
    {
        var key = attribute.Action;

        if (!_xmlHandlerMappings.ContainsKey(key))
        {
            _xmlHandlerMappings[key] = [];
        }

        if (attribute.Policy == EHandlerPolicy.Overwrite)
        {
            _xmlHandlerMappings[key].Clear();
        }

        _xmlHandlerMappings[key].Add(handlerType);
    }

    public List<IMessageHandler> GetXTHandlers(string id, string extension)
    {
        var key = (id, extension);

        if (_xtHandlerMappings.TryGetValue(key, out var handlerTypes))
        {
            return [.. handlerTypes.Select(ht => (IMessageHandler)_serviceProvider.GetRequiredService(ht))];
        }

        return [];
    }

    public List<IMessageHandler> GetXMLHandlers(string action)
    {
        if (_xmlHandlerMappings.TryGetValue(action, out var handlerTypes))
        {
            return [.. handlerTypes.Select(ht => (IMessageHandler)_serviceProvider.GetRequiredService(ht))];
        }

        return [];
    }

    public static List<Type> GetHandlerList(IEnumerable<Assembly> assemblies)
    {
        List<Type> handlerTypes = [];
        foreach (var assembly in assemblies)
        {
            handlerTypes.AddRange(assembly.GetTypes().Where(t => typeof(IMessageHandler).IsAssignableFrom(t) && !t.IsInterface));
        }
        return handlerTypes;
    }
}