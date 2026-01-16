using ResultOf;
namespace TagsCloudContainer.Core.Infrastructure.Preprocessing;

public class EmptyBoringWordsProvider : IBoringWordsProvider
{
    public Result<string[]> GetWords() => Result.Ok(Array.Empty<string>());
}