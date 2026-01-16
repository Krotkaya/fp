namespace TagsCloudContainer.Core.Infrastructure.Preprocessing;

public interface IBoringWordsProvider
{
    string[] GetWords();
}