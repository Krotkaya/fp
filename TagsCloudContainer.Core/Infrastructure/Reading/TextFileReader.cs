using ResultOf;

namespace TagsCloudContainer.Core.Infrastructure.Reading;
public class TextFileReader : IFileTextReader
{
    public bool CanRead(string filePath)
    {
        return filePath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase);
    }

    public Result<IReadOnlyList<string>> ReadWords(string filePath)
    {
        if (!File.Exists(filePath))
            return Result.Fail<IReadOnlyList<string>>($"File not found: {filePath}");
        
        return Result.Of<IReadOnlyList<string>>(() => File.ReadLines(filePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Trim()).ToArray(),
            $"Failed to read file: {filePath}");
    }
}