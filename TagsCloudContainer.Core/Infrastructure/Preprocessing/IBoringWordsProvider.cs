using ResultOf;
namespace TagsCloudContainer.Core.Infrastructure.Preprocessing;
public interface IBoringWordsProvider
{
    Result<string[]> GetWords();
}