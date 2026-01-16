using SkiaSharp;
using TagsCloudContainer.Core.Infrastructure.Coloring;
using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Core.Infrastructure.Layout;
public class TightSpiralTagCloudAlgorithm(
    IFontSizeCalculator fontSizeCalculator,
    IColorScheme colorScheme)
    : ITagCloudAlgorithm
{
    public IEnumerable<LayoutWord> Arrange(IEnumerable<WordFrequency>
        wordFrequencies, LayoutOptions options)
    {
        var frequencies = wordFrequencies.ToArray();
        if (frequencies.Length == 0)
            yield break;

        var maxFrequency = frequencies.Max(x => x.Frequency);
        var typeface = SKTypeface.FromFamilyName(options.FontFamily);
        var layouter = new RectangleCloudLayouter(options.Center, 
            new ArchimedeanSpiralPointGenerator(options.Center));

        foreach (var wordFrequency in frequencies)
        {
            var fontSize = fontSizeCalculator.Calculate(wordFrequency.Frequency,
                maxFrequency);
            var size = MeasureWordSize(wordFrequency.Word, typeface, fontSize);
            var rect = layouter.PutNextRectangle(size);
            var seed = wordFrequency.Word.GetHashCode();
            var color = colorScheme.GetColor(seed);

            yield return new LayoutWord(wordFrequency, typeface, fontSize, color, rect);
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
}
