using ResultOf;
using SkiaSharp;
using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Core.Infrastructure.Rendering;
public class PngRenderer : ITagCloudRenderer
{
    public  Result<SKImage> Render(IEnumerable<LayoutWord> layoutWords, LayoutOptions options)
    {
        return Result.Of(() =>
        {
            var info = new SKImageInfo(options.Width, options.Height);
            using var surface = SKSurface.Create(info);
            if (surface == null)
                throw new InvalidOperationException("Failed to create Skia surface");

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
        }, "Failed to render image");
    }

    public Result<None> SaveToFile(SKImage image, string filePath)
    {
        var dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
            return Result.Fail<None>($"Output directory doesn't exist {dir}");
        return Result.OfAction(() =>
            {
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);
                if (data == null)
                    throw new InvalidOperationException("Failed to encode image");
                using var stream = File.OpenWrite(filePath);
                data.SaveTo(stream);
            }, $"Failed to save file {filePath}");
    }
}