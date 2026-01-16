using SkiaSharp;

namespace TagsCloudContainer.Core.Models;
public class LayoutOptions
{
    public int Width { get; init; } = 800;
    public int Height { get; init; } = 600;
    public string FontFamily { get; init; } = "Arial";
    public SKColor BackgroundColor { get; } = SKColors.White;
    public SKPoint Center { get; }
    
    public LayoutOptions()
    {
        Center = new SKPoint(Width / 2f, Height / 2f);
    }
}