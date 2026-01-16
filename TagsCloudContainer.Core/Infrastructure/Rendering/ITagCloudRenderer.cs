using SkiaSharp;
using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Core.Infrastructure.Rendering;
public interface ITagCloudRenderer
{
    SKImage Render(IEnumerable<LayoutWord> layoutWords, LayoutOptions options);
    void SaveToFile(SKImage image, string filePath);
}