namespace TagsCloudContainer.Core.Infrastructure.Reading;
public class MultiFormatTextReader(IEnumerable<IFileTextReader> readers) : ITextReader
{
    private readonly IReadOnlyCollection<IFileTextReader> _readers = readers.ToArray();

    public bool CanRead(string filePath) => _readers.Any(r =>
        r.CanRead(filePath));

    public IReadOnlyList<string> ReadWords(string filePath)
    {
        var reader = _readers.FirstOrDefault(r => r.CanRead(filePath));
        return reader == null ? 
            throw new NotSupportedException($"No reader for file {filePath}") 
            : reader.ReadWords(filePath);
    }
}
