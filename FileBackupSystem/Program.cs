using System.Text.Json;
using FileBackupSystem;

try
{
    string json = File.ReadAllText("config.json");
    AppConfig config = JsonSerializer.Deserialize<AppConfig>(json)
                       ?? throw new Exception("Config is empty");

    Console.WriteLine("Configuration loaded:");
    Console.WriteLine($"Target folder: {config.TargetFolder}");
    Console.WriteLine($"Sources count: {config.SourceFolders.Count}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error loading config: {ex.Message}");
}