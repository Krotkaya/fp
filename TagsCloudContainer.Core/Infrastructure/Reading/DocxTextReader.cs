using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace TagsCloudContainer.Core.Infrastructure.Reading;
public class DocxTextReader : IFileTextReader
{
    public bool CanRead(string filePath) =>
        filePath.EndsWith(".docx", StringComparison.OrdinalIgnoreCase);

    public IReadOnlyList<string> ReadWords(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        using var doc = WordprocessingDocument.Open(filePath, false);
        var body = doc.MainDocumentPart?.Document?.Body;
        if (body == null)
            return [];

        var words = new List<string>();
        foreach (var line in body.Descendants<Text>().Select(t => t.Text))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;
            words.AddRange(line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        }

        return words;
    }
}
