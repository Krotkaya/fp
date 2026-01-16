using TagsCloudContainer.Core.Infrastructure.Analysis;
using TagsCloudContainer.Core.Infrastructure.Layout;
using TagsCloudContainer.Core.Infrastructure.Preprocessing;
using TagsCloudContainer.Core.Infrastructure.Reading;
using TagsCloudContainer.Core.Infrastructure.Rendering;
using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Console;
public class ConsoleClient(
    ITextReader textReader,
    ITextPreprocessor preprocessor,
    IWordFrequencyAnalyzer analyzer,
    ITagCloudAlgorithm algorithm,
    ITagCloudRenderer renderer)
{
    public void GenerateTagCloud(
        string inputFile,
        string outputFile,
        LayoutOptions options)
    {
        System.Console.WriteLine($"Reading words from {inputFile}...");
        var words = textReader.ReadWords(inputFile).ToArray();

        System.Console.WriteLine("Processing words...");
        var processedWords = preprocessor.Process(words).ToArray();

        System.Console.WriteLine("Analyzing word frequencies...");
        var frequencies = analyzer.Analyze(processedWords).ToArray();

        System.Console.WriteLine("Arranging words in cloud...");
        var layoutWords = algorithm.Arrange(frequencies, options).ToArray();

        System.Console.WriteLine($"Rendering image ({options.Width}x{options.Height})...");
        var image = renderer.Render(layoutWords, options);

        renderer.SaveToFile(image, outputFile);

        System.Console.WriteLine($"Tag cloud saved to {outputFile}");
        System.Console.WriteLine($"Total words processed: {frequencies.Length}");
    }
}