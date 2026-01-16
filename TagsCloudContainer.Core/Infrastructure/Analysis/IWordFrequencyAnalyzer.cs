using ResultOf;
using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Core.Infrastructure.Analysis;

public interface IWordFrequencyAnalyzer
{
    Result<IReadOnlyList<WordFrequency>> Analyze(IEnumerable<string> words);
}