using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace ShouldIDeployApp.Services;

/// <summary>
/// Generates a colored circle bitmap for use as the system tray icon.
/// White circle when it's safe to deploy, red (#FF4136) circle when it's not.
/// </summary>
public static class TrayIconHelper
{
    private const int IconSize = 64;

    /// <summary>
    /// Creates a WindowIcon containing a filled circle of the specified color.
    /// </summary>
    public static WindowIcon CreateCircleIcon(bool canDeploy)
    {
        var color = canDeploy ? Color.Parse("#FFFFFF") : Color.Parse("#FF4136");
        var borderColor = canDeploy ? Color.Parse("#CCCCCC") : Color.Parse("#FF4136");

        using var bitmap = new RenderTargetBitmap(new PixelSize(IconSize, IconSize), new Vector(96, 96));
        using (var ctx = bitmap.CreateDrawingContext())
        {
            // Draw filled circle
            var center = new Point(IconSize / 2.0, IconSize / 2.0);
            var radius = (IconSize / 2.0) - 2;
            var geometry = new EllipseGeometry(new Rect(
                center.X - radius, center.Y - radius,
                radius * 2, radius * 2));

            ctx.DrawGeometry(new SolidColorBrush(color), new Pen(new SolidColorBrush(borderColor), 2), geometry);
        }

        var stream = new MemoryStream();
        bitmap.Save(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return new WindowIcon(stream);
    }
}
