using SkiaSharp;
using TagsCloudContainer.Core.Infrastructure.Coloring;
using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Core.Infrastructure.Layout;
public class SpiralTagCloudAlgorithm(
    IFontSizeCalculator fontSizeCalculator,
    IColorScheme colorScheme)
    : ITagCloudAlgorithm
{
    public IEnumerable<LayoutWord> Arrange(
        IEnumerable<WordFrequency> wordFrequencies,
        LayoutOptions options)
    {
        var frequencies = wordFrequencies.ToArray();
        if (frequencies.Length == 0)
            yield break;

        var maxFrequency = frequencies.Max(wf => wf.Frequency);
        var placedRectangles = new List<SKRect>();
        var center = options.Center;
        var typeface = SKTypeface.FromFamilyName(options.FontFamily);

        foreach (var wordFrequency in frequencies)
        {
            var fontSize = fontSizeCalculator.Calculate(
                wordFrequency.Frequency, maxFrequency);
            
            var size = MeasureWordSize(wordFrequency.Word, typeface, fontSize);
            var location = FindLocationForRectangle(size, center, placedRectangles);
            
            if (location == SKPoint.Empty)
                continue;

            var rectangle = new SKRect(location.X, location.Y, 
                location.X + size.Width, location.Y + size.Height);
            placedRectangles.Add(rectangle);

            var seed = wordFrequency.Word.GetHashCode();
            var color = colorScheme.GetColor(seed);
            
            yield return new LayoutWord(wordFrequency, typeface, fontSize, color, rectangle);
        }
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
        SKPoint center, 
        List<SKRect> placedRectangles)
    {
        const double angleStep = 0.1;
        const double radiusStep = 1;
        
        double angle = 0;
        double radius = 0;

        for (var i = 0; i < 1000; i++)
        {
            var x = center.X + (float)(radius * Math.Cos(angle)) - size.Width / 2;
            var y = center.Y + (float)(radius * Math.Sin(angle)) - size.Height / 2;

            var candidate = new SKRect(x, y, x + size.Width, y + size.Height);

            if (!placedRectangles.Any(rect => rect.IntersectsWith(candidate)) &&
                candidate is { Left: >= 0, Top: >= 0 })
            {
                return new SKPoint(x, y);
            }
            angle += angleStep;
            radius += radiusStep;
        }

        return SKPoint.Empty;
    }
}

