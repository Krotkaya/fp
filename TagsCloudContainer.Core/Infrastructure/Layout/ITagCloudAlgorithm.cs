using ResultOf;
using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Core.Infrastructure.Layout;
public interface ITagCloudAlgorithm
{
    Result<IReadOnlyList<LayoutWord>> Arrange(
        IEnumerable<WordFrequency> wordFrequencies,
        LayoutOptions options);
}