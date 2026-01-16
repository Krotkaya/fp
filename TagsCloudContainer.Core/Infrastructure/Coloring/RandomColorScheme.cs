using SkiaSharp;

namespace TagsCloudContainer.Core.Infrastructure.Coloring;
public class RandomColorScheme : IColorScheme
{
    private readonly SKColor[] _colors =
    [
        SKColors.Red, SKColors.Blue, SKColors.Green, SKColors.Purple,
        SKColors.Orange, new(139, 0, 0), 
        new(0, 0, 139), 
        new(0, 100, 0), 
        new(165, 42, 42), 
        new(0, 139, 139),
        new(139, 0, 139)
    ];

    public SKColor GetColor(int seed)
    {
        var index = Math.Abs(seed) % _colors.Length; 
        return _colors[index];
    }
}