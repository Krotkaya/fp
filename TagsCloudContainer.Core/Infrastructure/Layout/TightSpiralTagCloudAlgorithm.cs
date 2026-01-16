using ResultOf;
using SkiaSharp;
using TagsCloudContainer.Core.Infrastructure.Coloring;
using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Core.Infrastructure.Layout;
public class TightSpiralTagCloudAlgorithm(
    IFontSizeCalculator fontSizeCalculator,
    IColorScheme colorScheme)
    : ITagCloudAlgorithm
{
    public Result<IReadOnlyList<LayoutWord>> Arrange(IEnumerable<WordFrequency>
        wordFrequencies, LayoutOptions options)
    {
        var frequencies = wordFrequencies.ToArray();
        if (frequencies.Length == 0)
            return Result.Ok<IReadOnlyList<LayoutWord>>([]);

        var typeface = SKFontManager.Default.MatchFamily(options.FontFamily);
        if (typeface == null)
            return Result.Fail<IReadOnlyList<LayoutWord>>($"Font'{options.FontFamily}' not found");

        var bounds = new SKRect(0, 0, options.Width, options.Height);
        var layouter = new RectangleCloudLayouter(options.Center,
            new ArchimedeanSpiralPointGenerator(options.Center));

        var maxFrequency = frequencies.Max(x => x.Frequency);
        var layoutWords = new List<LayoutWord>();

        foreach (var wordFrequency in frequencies)
        {
            var fontSize = fontSizeCalculator.Calculate(wordFrequency.Frequency, 
                maxFrequency);
            var size = MeasureWordSize(wordFrequency.Word, typeface, fontSize);
            var rectResult = layouter.PutNextRectangle(size, bounds);
            if (!rectResult.IsSuccess)
                return Result.Fail<IReadOnlyList<LayoutWord>>(rectResult.Error);

            var rect = rectResult.GetValueOrThrow();
            var color =
                colorScheme.GetColor(wordFrequency.Word.GetHashCode());

            layoutWords.Add(new LayoutWord(wordFrequency, typeface, fontSize,
                color, rect));
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
}
