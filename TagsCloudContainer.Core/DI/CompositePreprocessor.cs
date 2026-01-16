using TagsCloudContainer.Core.Infrastructure.Preprocessing;

namespace TagsCloudContainer.Core.DI;
public class CompositePreprocessor(IEnumerable<ITextPreprocessor> preprocessors) : ITextPreprocessor
{
    public IReadOnlyList<string> Process(IEnumerable<string> words)
    {
        return preprocessors.Aggregate(words, (current, preprocessor) 
            => preprocessor.Process(current)).ToArray();
    }
}