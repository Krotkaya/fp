using ResultOf;
using SkiaSharp;
using TagsCloudContainer.Core.Infrastructure.Coloring;
using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Core.Infrastructure.Layout;
public class SpiralTagCloudAlgorithm(
    IFontSizeCalculator fontSizeCalculator,
    IColorScheme colorScheme)
    : ITagCloudAlgorithm
{
    public Result<IReadOnlyList<LayoutWord>> Arrange(
        IEnumerable<WordFrequency> wordFrequencies,
        LayoutOptions options)
    {
        var frequencies = wordFrequencies.ToArray();
        if (frequencies.Length == 0)
            return
                Result.Ok<IReadOnlyList<LayoutWord>>([]);

        var typeface = SKFontManager.Default.MatchFamily(options.FontFamily);
        if (typeface == null)
            return Result.Fail<IReadOnlyList<LayoutWord>>($"Font'{options.FontFamily}' not found.");

        var maxFrequency = frequencies.Max(wf => wf.Frequency);
        var placedRectangles = new List<SKRect>();
        var layoutWords = new List<LayoutWord>();
        var bounds = new SKRect(0, 0, options.Width, options.Height);

        foreach (var wordFrequency in frequencies)
        {
            var fontSize = fontSizeCalculator.Calculate(
                wordFrequency.Frequency, maxFrequency);
            var size = MeasureWordSize(wordFrequency.Word, typeface, fontSize);
            var location = FindLocationForRectangle(size, options, bounds, placedRectangles);
            if (location == SKPoint.Empty)
                return Result.Fail<IReadOnlyList<LayoutWord>>(
                    "Tag cloud doesn't fit the image");

            var rectangle = new SKRect(location.X, location.Y, 
                location.X + size.Width, location.Y + size.Height);
            placedRectangles.Add(rectangle);
            var color = colorScheme.GetColor(wordFrequency.Word.GetHashCode());
            layoutWords.Add(new LayoutWord(wordFrequency, typeface, fontSize, color, rectangle));
        }
        return Result.Ok<IReadOnlyList<LayoutWord>>(layoutWords);
    }

    private static SKSize MeasureWordSize(
        string word, 
        SKTypeface typeface, 
        float fontSize)
    {
        using var paint = new SKPaint();
        paint.Typeface = typeface;
        paint.TextSize = fontSize;
        var bounds = new SKRect();
        paint.MeasureText(word, ref bounds);
        return new SKSize(bounds.Width, bounds.Height);
    }
    
    private static SKPoint FindLocationForRectangle(
        SKSize size,
        LayoutOptions options,
        SKRect bounds,
        List<SKRect> placedRectangles)
    {
        const double angleStep = 0.1;
        const double radiusStep = 1;
        double angle = 0;
        double radius = 0;
        for (var i = 0; i < 1000; i++)
        {
            var x = options.Center.X + (float)(radius * Math.Cos(angle)) - size.Width / 2;
            var y = options.Center.Y + (float)(radius * Math.Sin(angle)) - size.Height / 2;
            var candidate = new SKRect(x, y, x + size.Width, y + size.Height);
            var fits = candidate.Left >= bounds.Left &&
                       candidate.Top >= bounds.Top &&
                       candidate.Right <= bounds.Right &&
                       candidate.Bottom <= bounds.Bottom;
            if (fits && !placedRectangles.Any(rect =>
                    rect.IntersectsWith(candidate)))
                return new SKPoint(x, y);

            angle += angleStep;
            radius += radiusStep;
        }
        return SKPoint.Empty;
    }
}

