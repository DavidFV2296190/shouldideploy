using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace ShouldIDeployApp.Services;

/// <summary>
/// Generates an emoji-based bitmap for use as the system tray icon.
/// Shows ✅ when it's safe to deploy, ⛔ when it's not.
/// </summary>
public static class TrayIconHelper
{
    private const int IconSize = 64;

    /// <summary>
    /// Creates a WindowIcon displaying ✅ or ⛔ based on deployment status.
    /// </summary>
    public static WindowIcon CreateStatusIcon(bool canDeploy)
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

            // Center the emoji in the icon
            var x = (IconSize - formattedText.Width) / 2;
            var y = (IconSize - formattedText.Height) / 2;
            ctx.DrawText(formattedText, new Point(x, y));
        }

        var stream = new MemoryStream();
        bitmap.Save(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return new WindowIcon(stream);
    }
}
