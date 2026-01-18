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

    Console.WriteLine("Configuration loaded successfully");
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
Console.WriteLine($"Backup folder created: {backupFolder}");

// =======================
// Process source folders
// =======================
foreach (var sourceFolder in config.SourceFolders)
{
    Console.WriteLine($"Processing source folder: {sourceFolder}");

    if (!Directory.Exists(sourceFolder))
    {
        Console.WriteLine($"Source folder does not exist: {sourceFolder}");
        continue;
    }

    foreach (var file in Directory.GetFiles(sourceFolder))
    {
        try
        {
            string fileName = Path.GetFileName(file);
            string destination = Path.Combine(backupFolder, fileName);

            File.Copy(file, destination, overwrite: true);
            Console.WriteLine($"Copied: {fileName}");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"No access to file: {file}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"IO error with file {file}: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error with file {file}: {ex.Message}");
        }
    }
}

Console.WriteLine("Backup completed");