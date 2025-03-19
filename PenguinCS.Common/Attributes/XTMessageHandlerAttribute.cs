using System;
using PenguinCS.Common.Enums;

namespace PenguinCS.Common.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class XTMessageHandlerAttribute(string id, string ext, EHandlerPolicy policy = EHandlerPolicy.Append) : MessageHandlerAttribute(EMessageFormat.XT, policy)
{
    public string Id { get; } = id;
    public string Extension { get; } = ext;
}