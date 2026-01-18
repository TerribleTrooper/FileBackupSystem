namespace FileBackupSystem;
using System.Collections.Generic;

public class AppConfig
{
    public List<string> SourceFolders { get; set; } = new();
    public string TargetFolder { get; set; } = string.Empty;
    public string LogLevel { get; set; } = "Info";
}
