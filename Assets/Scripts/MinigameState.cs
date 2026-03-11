using System.Collections.Generic;

public static class MinigameState
{
    public static string InitialState = "";
    public static string ReturnScene = "";
    public static Vector2 ReturnPosition;

    public static Dictionary<string, bool> CompletionStatus  = new Dictionary<string, int>()
    {
        { "Terminal1", false },
        { "Terminal2", false }
    };


}
