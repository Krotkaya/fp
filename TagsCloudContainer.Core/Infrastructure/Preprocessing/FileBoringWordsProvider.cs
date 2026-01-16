using ResultOf;
namespace TagsCloudContainer.Core.Infrastructure.Preprocessing;

public class FileBoringWordsProvider(string path) : IBoringWordsProvider
{
    public Result<string[]> GetWords()  
    {
        if (string.IsNullOrWhiteSpace(path))
            return Result.Ok(Array.Empty<string>());

        if (!File.Exists(path))
            return Result.Fail<string[]>($"Boring words file not found {path}");

        return Result.Of(() => File.ReadAllLines(path),
            $"Failed to read boring words file {path}");
    }

}