namespace TagsCloudContainer.Core.Infrastructure.Preprocessing;

public class FileBoringWordsProvider(string path) : IBoringWordsProvider
{
    public string[] GetWords() => File.ReadAllLines(path);
}