using UnityEngine;

public static class LogColorUtility
{
    // 扩展方法，用于给字符串设置颜色
    public static string SetColor(this object text, Color color)
    {
        // 将 Color 转成十六进制
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        return $"<color=#{hexColor}>{text}</color>";
    }

    // 可选：支持 Debug.Log 彩色输出
    public static void Log(string text)
    {
        Debug.Log(text);
    }

    public static void Log(string text, Color color)
    {
        Debug.Log(text.SetColor(color));
    }

    public static void LogFormat(string format, params object[] args)
    {
        Debug.Log(string.Format(format, args));
    }
}
