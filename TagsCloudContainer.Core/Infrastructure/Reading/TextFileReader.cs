namespace TagsCloudContainer.Core.Infrastructure.Reading;
public class TextFileReader : IFileTextReader
{
    public bool CanRead(string filePath)
    {
        return filePath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase);
    }

    public IReadOnlyList<string> ReadWords(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        return File.ReadLines(filePath)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Trim()).ToArray();
    }
}