using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Core.Infrastructure.Layout;
public interface ITagCloudAlgorithm
{
    IEnumerable<LayoutWord> Arrange(
        IEnumerable<WordFrequency> wordFrequencies,
        LayoutOptions options);
}