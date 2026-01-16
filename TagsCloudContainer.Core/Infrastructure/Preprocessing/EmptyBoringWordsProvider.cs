namespace TagsCloudContainer.Core.Infrastructure.Preprocessing;

public class EmptyBoringWordsProvider : IBoringWordsProvider
{
    public string[] GetWords() => [];
}