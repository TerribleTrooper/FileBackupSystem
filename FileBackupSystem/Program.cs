using System;
using System.IO;
using System.Text.Json;
using FileBackupSystem;

// =======================
// Load configuration
// =======================
AppConfig? config = null;

try
{
    string json = File.ReadAllText("config.json");
    config = JsonSerializer.Deserialize<AppConfig>(json)
             ?? throw new Exception("Config is empty");
}
catch (Exception ex)
{
    Console.WriteLine($"Error loading config: {ex.Message}");
    return;
}

// =======================
// Create backup folder
// =======================
string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

string backupFolder = Path.Combine(
    config.TargetFolder,
    timestamp
);

Directory.CreateDirectory(backupFolder);

// =======================
// Setup logger
// =======================
LogLevel logLevel =
    Enum.TryParse(config.LogLevel, true, out LogLevel parsedLevel)
        ? parsedLevel
        : LogLevel.Info;

string logFilePath = Path.Combine(backupFolder, "backup.log");
var logger = new Logger(logLevel, logFilePath);

logger.Log(LogLevel.Info, "Backup application started");
logger.Log(LogLevel.Info, $"Backup folder: {backupFolder}");

// =======================
// Process source folders
// =======================
foreach (var sourceFolder in config.SourceFolders)
{
    logger.Log(LogLevel.Info, $"Processing source folder: {sourceFolder}");

    if (!Directory.Exists(sourceFolder))
    {
        logger.Log(LogLevel.Error, $"Source folder does not exist: {sourceFolder}");
        continue;
    }

    var files = Directory.GetFiles(sourceFolder);
    logger.Log(LogLevel.Debug, $"Found {files.Length} files in {sourceFolder}");

    foreach (var file in files)
    {
        try
        {
            string fileName = Path.GetFileName(file);
            string destination = Path.Combine(backupFolder, fileName);

            File.Copy(file, destination, overwrite: true);
            logger.Log(LogLevel.Debug, $"Copied file: {fileName}");
        }
        catch (UnauthorizedAccessException)
        {
            logger.Log(LogLevel.Error, $"No access to file: {file}");
        }
        catch (IOException ex)
        {
            logger.Log(LogLevel.Error, $"IO error with file {file}: {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, $"Unexpected error with file {file}: {ex.Message}");
        }
    }
}

logger.Log(LogLevel.Info, "Backup completed");
