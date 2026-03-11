using System.Collections.Generic;
using UnityEngine;

public static class MinigameState
{
    public static string TerminalID = "";
    public static string InitialState = "";
    public static string ReturnScene = "";
    public static Vector2 ReturnPosition;

    public static Dictionary<string, bool> CompletionStatus  = new Dictionary<string, bool>()
    {
        { "Terminal1", false },
        { "Terminal2", false }
    };


}
