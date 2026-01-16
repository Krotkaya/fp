using ResultOf;
using TagsCloudContainer.Core.Infrastructure.Preprocessing;

namespace TagsCloudContainer.Core.DI;
public class CompositePreprocessor(IEnumerable<ITextPreprocessor> preprocessors) : ITextPreprocessor
{
    public Result<IReadOnlyList<string>> Process(IEnumerable<string> words)
    {
        var current = Result.Ok<IReadOnlyList<string>>(words.ToArray());
        return preprocessors.Aggregate(current, (current1, preprocessor) 
            => current1.Then(preprocessor.Process));
    }


}