namespace PenguinCS.Common;

public static class XTMessage
{
    public static string UnknownError => CreateError(0);

    public static string CreateMessage(string group, params string[] args)
    {
        var argString = string.Join('%', args);
        string internalId = (-1).ToString();

        string message = string.Format("%xt%{0}%{1}%{2}%", group, internalId, argString);

        return message;
    }

    public static string CreateError(int errorCode, params string[] args)
    {
        args = [errorCode.ToString(), .. args];

        return CreateMessage("e", args);
    }
}