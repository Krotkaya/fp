using ResultOf;
namespace TagsCloudContainer.Core.Infrastructure.Preprocessing;
public class LowerCaseNormalizer : ITextPreprocessor
{
    public Result<IReadOnlyList<string>> Process(IEnumerable<string> words) =>
        Result.Ok<IReadOnlyList<string>>(words.Select(w => 
            w.ToLowerInvariant()).ToArray());
}