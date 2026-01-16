using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Core.Infrastructure.Analysis;
public class WordFrequencyAnalyzer : IWordFrequencyAnalyzer
{ 
    public  IReadOnlyList<WordFrequency> Analyze(IEnumerable<string> words)
    {
        var frequencies = words
            .GroupBy(word => word)
            .Select(group 
                => new WordFrequency(group.Key, group.Count()))
            .OrderByDescending(wf => wf.Frequency).ToArray();
        return frequencies;
    }
}