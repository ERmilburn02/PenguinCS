namespace PenguinCS.Common;

public static class XTMessage
{
    public static string UnknownError => CreateError(0);
    
    /// <summary>
    /// Listed as "No db connection" on the Wiki, shows as generic "There was an error" in-game
    /// </summary>
    public static string DatabaseError => CreateError(1000);

    public static string CreateMessage(string id, params string[] args)
    {
        var argString = string.Join('%', args);
        string internalId = (-1).ToString();

        string message = string.Format("%xt%{0}%{1}%{2}%", id, internalId, argString);

        return message;
    }

    public static string CreateError(int errorCode, params string[] args)
    {
        args = [errorCode.ToString(), .. args];

        return CreateMessage("e", args);
    }
}