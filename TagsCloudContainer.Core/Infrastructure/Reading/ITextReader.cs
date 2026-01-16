namespace TagsCloudContainer.Core.Infrastructure.Reading;

public interface ITextReader
{
    bool CanRead(string filePath);
    IReadOnlyList<string> ReadWords(string filePath);
}

public interface IFileTextReader : ITextReader
{ }