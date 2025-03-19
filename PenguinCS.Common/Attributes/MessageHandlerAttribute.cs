using System;
using PenguinCS.Common.Enums;

namespace PenguinCS.Common.Attributes;

public abstract class MessageHandlerAttribute(EMessageFormat format, EHandlerPolicy policy = EHandlerPolicy.Append) : Attribute
{
    public EMessageFormat Format { get; } = format;
    public EHandlerPolicy Policy { get; } = policy;
}