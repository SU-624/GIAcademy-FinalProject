using System.Diagnostics;
using Unity.Collections;
using UnityEngine.TerrainTools; // Debug 클래스가 있는 곳.

internal static class Debug
{
    private const string DefaultTag = "NoTag";

    public static void Log(object message, bool writeToFile = false)
    {
        UnityEngine.Debug.Log(message);
        if(writeToFile)
            Logger.AppendLog(DefaultTag, message.ToString(), LogLevel.Debug);
    }

    public static void Log(string tag, object message, bool writeToFile = false)
    {
        UnityEngine.Debug.Log(message);
        if (writeToFile)
            Logger.AppendLog(tag, message.ToString(), LogLevel.Debug);
    }

    public static void LogError(object message, bool writeToFile = false)
    {
        UnityEngine.Debug.Log(message);
        if (writeToFile)
            Logger.AppendLog(DefaultTag, message.ToString(), LogLevel.Error);
    }

    public static void LogError(string tag, object message, bool writeToFile = false)
    {
        UnityEngine.Debug.Log(message);
        if (writeToFile)
            Logger.AppendLog(tag, message.ToString(), LogLevel.Error);
    }

    public static void LogWarning(object message, bool writeToFile = false)
    {
        UnityEngine.Debug.Log((message));
        if (writeToFile)
            Logger.AppendLog(DefaultTag, message.ToString(), LogLevel.Warning);
    } 

    public static void LogWarning(string tag, object message, bool writeToFile = false)
    {
        UnityEngine.Debug.Log((message));
        if (writeToFile)
            Logger.AppendLog(tag, message.ToString(), LogLevel.Warning);
    }
}