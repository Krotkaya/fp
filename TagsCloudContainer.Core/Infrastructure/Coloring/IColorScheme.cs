using SkiaSharp;

namespace TagsCloudContainer.Core.Infrastructure.Coloring;
public interface IColorScheme
{
    SKColor GetColor(int seed);
}