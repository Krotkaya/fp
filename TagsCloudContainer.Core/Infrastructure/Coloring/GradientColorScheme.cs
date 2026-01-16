using SkiaSharp;

namespace TagsCloudContainer.Core.Infrastructure.Coloring;
public class GradientColorScheme(SKColor startColor, SKColor endColor) : IColorScheme
{
    public SKColor GetColor(int seed)
    {
        var hash = Math.Abs(seed);
        var ratio = (hash % 100) / 100.0f;
        
        return InterpolateColor(startColor, endColor, ratio);
    }

    private static SKColor InterpolateColor(SKColor start, SKColor end, float ratio)
    {
        var r = (byte)(start.Red + (end.Red - start.Red) * ratio);
        var g = (byte)(start.Green + (end.Green - start.Green) * ratio);
        var b = (byte)(start.Blue + (end.Blue - start.Blue) * ratio);
        
        return new SKColor(r, g, b);
    }
}