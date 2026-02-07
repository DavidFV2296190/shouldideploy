using System.Text.Json;

namespace ShouldIDeployApp.Services;

/// <summary>
/// Persists user settings to a JSON file in the user's home directory.
/// Settings file location: {UserProfile}/ShouldIDeployApp/settings.json
/// </summary>
public class AppSettings
{
    private static readonly string SettingsDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        "ShouldIDeployApp");

    private static readonly string SettingsFilePath = Path.Combine(
        SettingsDirectory, "settings.json");

    public string Timezone { get; set; } = "UTC";
    public bool IsFullScreen { get; set; }

    public static AppSettings Load()
    {
        try
        {
            if (File.Exists(SettingsFilePath))
            {
                var json = File.ReadAllText(SettingsFilePath);
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
        }
        catch (Exception ex) when (ex is System.Text.Json.JsonException or IOException)
        {
            // If settings file is corrupt or unreadable, return defaults
        }

        return new AppSettings();
    }

    public void Save()
    {
        try
        {
            Directory.CreateDirectory(SettingsDirectory);
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFilePath, json);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Text.Json.JsonException)
        {
            // Best effort - don't crash the app if settings can't be saved
        }
    }

    /// <summary>
    /// Returns the directory where settings are stored, for testing/documentation purposes.
    /// </summary>
    public static string GetSettingsDirectory() => SettingsDirectory;

    /// <summary>
    /// Returns the full path to the settings file.
    /// </summary>
    public static string GetSettingsFilePath() => SettingsFilePath;
}
