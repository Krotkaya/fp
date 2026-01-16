using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Core.Infrastructure.Analysis;

public interface IWordFrequencyAnalyzer
{
    IReadOnlyList<WordFrequency> Analyze(IEnumerable<string> words);
}