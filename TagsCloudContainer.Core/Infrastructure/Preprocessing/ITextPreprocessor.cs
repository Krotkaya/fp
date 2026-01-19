using ResultOf;

namespace TagsCloudContainer.Core.Infrastructure.Preprocessing;
public interface ITextPreprocessor
{
    Result<IReadOnlyList<string>> Process(IEnumerable<string> words);
}