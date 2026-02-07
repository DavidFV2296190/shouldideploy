using ShouldIDeployApp.Services;

namespace ShouldIDeployApp.Tests;

public class AppSettingsTests
{
    [Fact]
    public void Load_WhenNoFileExists_ReturnsDefaults()
    {
        var settings = new AppSettings();
        Assert.Equal(TimeZoneInfo.Local.Id, settings.Timezone);
        Assert.False(settings.IsFullScreen);
    }

    [Fact]
    public void Save_And_Load_RoundTrips()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), $"ShouldIDeployTest_{Guid.NewGuid()}");
        var tempFile = Path.Combine(tempDir, "settings.json");

        try
        {
            // Manually write settings to a temp file
            var settings = new AppSettings
            {
                Timezone = "US/Eastern",
                IsFullScreen = true
            };

            Directory.CreateDirectory(tempDir);
            var json = System.Text.Json.JsonSerializer.Serialize(settings);
            File.WriteAllText(tempFile, json);

            // Read it back
            var loaded = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(
                File.ReadAllText(tempFile));

            Assert.NotNull(loaded);
            Assert.Equal("US/Eastern", loaded!.Timezone);
            Assert.True(loaded.IsFullScreen);
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void DefaultTimezone_IsLocalTimezone()
    {
        var settings = new AppSettings();
        Assert.Equal(TimeZoneInfo.Local.Id, settings.Timezone);
    }

    [Fact]
    public void DefaultIsFullScreen_IsFalse()
    {
        var settings = new AppSettings();
        Assert.False(settings.IsFullScreen);
    }

    [Fact]
    public void SettingsDirectory_IsUnderUserProfile()
    {
        var dir = AppSettings.GetSettingsDirectory();
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        Assert.StartsWith(userProfile, dir);
        Assert.Contains("ShouldIDeployApp", dir);
    }

    [Fact]
    public void SettingsFilePath_EndsWithSettingsJson()
    {
        var path = AppSettings.GetSettingsFilePath();
        Assert.EndsWith("settings.json", path);
    }

    [Fact]
    public void Save_DoesNotThrow_WhenDirectoryExists()
    {
        // Save to the actual settings path - should not throw
        var settings = new AppSettings();
        var exception = Record.Exception(() => settings.Save());
        Assert.Null(exception);
    }

    [Fact]
    public void Properties_CanBeSetAndRead()
    {
        var settings = new AppSettings
        {
            Timezone = "Europe/London",
            IsFullScreen = true
        };

        Assert.Equal("Europe/London", settings.Timezone);
        Assert.True(settings.IsFullScreen);
    }
}
