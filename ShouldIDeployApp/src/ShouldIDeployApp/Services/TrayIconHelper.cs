using Avalonia.Controls;

namespace ShouldIDeployApp.Services;

/// <summary>
/// Loads the dots.png or dots-red.png icon for use as the system tray icon.
/// dots.png when it's safe to deploy, dots-red.png when it's not.
/// </summary>
public static class TrayIconHelper
{
    /// <summary>
    /// Creates a WindowIcon from the embedded dots.png or dots-red.png asset.
    /// </summary>
    public static WindowIcon CreateStatusIcon(bool canDeploy)
    {
        var assetPath = canDeploy ? "avares://ShouldIDeployApp/Assets/dots.png" : "avares://ShouldIDeployApp/Assets/dots-red.png";
        var uri = new Uri(assetPath);
        var stream = Avalonia.Platform.AssetLoader.Open(uri);
        return new WindowIcon(stream);
    }
}
