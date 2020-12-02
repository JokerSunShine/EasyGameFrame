
using UnityEngine;

public static class CSDebug {
    /// <summary>
    /// 日志开关
    /// </summary>
    public static bool LogSwitch = true;

    public static void Log(object message)
    {
        if (LogSwitch == false) {
            return;
        }
        UnityEngine.Debug.Log(message);
    }

    public static void Log(object message, Object context)
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.Log(message, context);
    }

    public static void LogWarning(object message)
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.LogWarning(message);
    }

    public static void LogWarning(object message, Object context)
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.LogWarning(message, context);
    }

    public static void LogError(object message)
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.LogError(message);
    }

    public static void LogError(object message, Object context)
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.LogError(message, context);
    }

    public static void LogFormat(string format, params object[] args)
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.LogFormat(format, args);
    }

    public static void LogFormat(Object context, string format, params object[] args)
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.LogFormat(context, format, args);
    }

    public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.LogFormat(logType, logOptions, context, format, args);
    }

    public static void LogWarningFormat(string format, params object[] args)
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.LogWarningFormat(format, args);
    }

    public static void LogWarningFormat(Object context, string format, params object[] args)
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.LogWarningFormat(context, format, args);
    }

    public static void LogErrorFormat(string format, params object[] args)
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.LogErrorFormat(format, args);
    }

    public static void LogErrorFormat(Object context, string format, params object[] args)
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.LogErrorFormat(context, format, args);
    }

    public static void LogException(System.Exception ex)
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.LogException(ex);
    }

    public static void LogException(System.Exception exception, Object context)
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.LogException(exception, context);
    }

    public static void Break()
    {
        if (LogSwitch == false)
        {
            return;
        }
        UnityEngine.Debug.Break();
    }
}