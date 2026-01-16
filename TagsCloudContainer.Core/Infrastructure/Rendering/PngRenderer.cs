using SkiaSharp;
using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Core.Infrastructure.Rendering;
public class PngRenderer : ITagCloudRenderer
{
    public SKImage Render(IEnumerable<LayoutWord> layoutWords, LayoutOptions options)
    {
        var info = new SKImageInfo(options.Width, options.Height);
        using var surface = SKSurface.Create(info);
        var canvas = surface.Canvas;
        
        canvas.Clear(options.BackgroundColor);
        
        foreach (var layoutWord in layoutWords)
        {
            using var paint = new SKPaint();
            paint.Color = layoutWord.Color;
            paint.Typeface = layoutWord.Typeface;
            paint.TextSize = layoutWord.FontSize;
            paint.IsAntialias = true;

            canvas.DrawText(
                layoutWord.WordFrequency.Word,
                layoutWord.Rectangle.Left,
                layoutWord.Rectangle.Bottom, 
                paint);
        }
        return surface.Snapshot();
    }

    public void SaveToFile(SKImage image, string filePath)
    {
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.OpenWrite(filePath);
        data.SaveTo(stream);
    }
}