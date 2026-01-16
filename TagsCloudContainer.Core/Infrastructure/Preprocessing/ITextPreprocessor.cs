namespace TagsCloudContainer.Core.Infrastructure.Preprocessing;
public interface ITextPreprocessor
{
    IReadOnlyList<string> Process(IEnumerable<string> words);
}