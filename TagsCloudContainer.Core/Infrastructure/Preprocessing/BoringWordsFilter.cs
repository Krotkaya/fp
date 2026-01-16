using ResultOf;
namespace TagsCloudContainer.Core.Infrastructure.Preprocessing;
public class BoringWordsFilter : ITextPreprocessor
{
    private readonly IBoringWordsProvider provider;

    public BoringWordsFilter(IBoringWordsProvider provider)
    {
        this.provider = provider;
    }

    public Result<IReadOnlyList<string>> Process(IEnumerable<string> words)
    {
        return provider.GetWords().Then(boringWords =>
        {
            var set = new HashSet<string>(boringWords,
                StringComparer.OrdinalIgnoreCase);
            var filtered = words.Where(word => !set.Contains(word)).ToArray();
            return Result.Ok<IReadOnlyList<string>>(filtered);
        });
    }
}