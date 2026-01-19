using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ResultOf;

namespace TagsCloudContainer.Core.Infrastructure.Reading;
public class DocxTextReader : IFileTextReader
{
    public bool CanRead(string filePath) =>
        filePath.EndsWith(".docx", StringComparison.OrdinalIgnoreCase);
    public Result<IReadOnlyList<string>> ReadWords(string filePath)
    {
        if (!File.Exists(filePath))
            return Result.Fail<IReadOnlyList<string>>($"File not found: {filePath}");
        
        return Result.Of(() =>
            {
                using var doc = WordprocessingDocument.Open(filePath, false);
                var body = doc.MainDocumentPart?.Document.Body;
                if (body == null)
                    return [];
                var words = new List<string>();
                foreach (var line in body.Descendants<Text>().Select(t => t.Text))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    words.AddRange(line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
                }
                return (IReadOnlyList<string>)words;
            })
            .RefineError($"Failed to read docx file: {filePath}");
    }
}
