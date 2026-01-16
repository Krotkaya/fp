using ResultOf;
using SkiaSharp;
using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Core.Infrastructure.Rendering;
public interface ITagCloudRenderer
{
    Result<SKImage> Render(IEnumerable<LayoutWord> layoutWords, LayoutOptions options);
    Result<None> SaveToFile(SKImage image, string filePath);
}