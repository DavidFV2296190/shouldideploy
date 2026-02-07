using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace ShouldIDeployApp.Services;

/// <summary>
/// Creates icons for the system tray and the application window/taskbar.
/// Tray icon: ✅ or ⛔ emoji.
/// Window/taskbar icon: dots.png or dots-red.png.
/// </summary>
public static class TrayIconHelper
{
    private const int IconSize = 64;

    /// <summary>
    /// Creates a WindowIcon displaying ✅ or ⛔ for the system tray.
    /// </summary>
    public static WindowIcon CreateTrayIcon(bool canDeploy)
    {
        var emoji = canDeploy ? "✅" : "⛔";

        using var bitmap = new RenderTargetBitmap(new PixelSize(IconSize, IconSize), new Vector(96, 96));
        using (var ctx = bitmap.CreateDrawingContext())
        {
            var formattedText = new FormattedText(
                emoji,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                Typeface.Default,
                48,
                Brushes.Black);

            var x = (IconSize - formattedText.Width) / 2;
            var y = (IconSize - formattedText.Height) / 2;
            ctx.DrawText(formattedText, new Point(x, y));
        }

        var stream = new MemoryStream();
        bitmap.Save(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return new WindowIcon(stream);
    }

    /// <summary>
    /// Creates a WindowIcon from dots.png or dots-red.png for the application window/taskbar.
    /// </summary>
    public static WindowIcon CreateWindowIcon(bool canDeploy)
    {
        var assetPath = canDeploy
            ? "avares://ShouldIDeployApp/Assets/dots.png"
            : "avares://ShouldIDeployApp/Assets/dots-red.png";
        var stream = Avalonia.Platform.AssetLoader.Open(new Uri(assetPath));
        return new WindowIcon(stream);
    }
}
