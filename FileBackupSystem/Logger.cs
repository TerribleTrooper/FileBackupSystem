using System;
using System.IO;

public enum LogLevel
{
    Error = 0,
    Info = 1,
    Debug = 2
}

public class Logger
{
    private readonly LogLevel _level;
    private readonly string _logFilePath;

    public Logger(LogLevel level, string logFilePath)
    {
        _level = level;
        _logFilePath = logFilePath;
    }

    public void Log(LogLevel level, string message)
    {
        if (level > _level)
            return;

        string line =
            $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";

        File.AppendAllText(_logFilePath, line + Environment.NewLine);
    }
}