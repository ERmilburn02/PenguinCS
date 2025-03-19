using PenguinCS.Common.Enums;

namespace PenguinCS.Common.Attributes;

public class XMLMessageHandlerAttribute(string action, EHandlerPolicy policy = EHandlerPolicy.Append) : MessageHandlerAttribute(EMessageFormat.XML, policy)
{
    public string Action { get; } = action;
}