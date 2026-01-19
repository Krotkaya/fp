using ResultOf;
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
    public Result<None> GenerateTagCloud(
        string inputFile,
        string outputFile,
        LayoutOptions options)
    {
        System.Console.WriteLine($"Reading words from {inputFile}...");

        return textReader.ReadWords(inputFile)
            .Then(preprocessor.Process)
            .Then(EnsureNotEmpty)
            .Then(analyzer.Analyze)
            .Then(freqs => 
                algorithm.Arrange(freqs, options))
            .Then(layout =>
            {
                System.Console.WriteLine($"Rendering image ({options.Width} x{options.Height})..."); 
                return renderer.Render(layout, options);
            })
            .Then(image => renderer.SaveToFile(image, outputFile))
            .RefineError("Tag cloud generation failed");
    }
    private static Result<IReadOnlyList<string>> EnsureNotEmpty(IReadOnlyList<string> words)
    {
        return words.Count == 0 
            ? Result.Fail<IReadOnlyList<string>>("No words to build a tag cloud") 
            : Result.Ok(words);
    }
}