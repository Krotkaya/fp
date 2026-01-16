namespace TagsCloudContainer.Core.Infrastructure.Preprocessing;
public class LowerCaseNormalizer : ITextPreprocessor
{
    public IReadOnlyList<string> Process(IEnumerable<string> words)
    {
        return words.Select(word => word.ToLowerInvariant()).ToArray();
    }
}