using CommandLine;
using TagsCloudContainer.Core.Infrastructure.Coloring;
using TagsCloudContainer.Core.Infrastructure.Layout;

namespace TagsCloudContainer.Console;
public class CommandLineOptions
{
    [Option('i', "input", Required = true, HelpText = "Input file path")]
    public string InputFile { get; init; } = string.Empty;

    [Option('o', "output", Required = true, HelpText = "Output file path")]
    public string OutputFile { get; init; } = string.Empty;

    [Option('w', "width", Default = 800, HelpText = "Image width")]
    public int Width { get; init; }

    [Option('h', "height", Default = 600, HelpText = "Image height")]
    public int Height { get; init; }

    [Option('f', "font", Default = "Arial", HelpText = "Font family")]
    public string FontFamily { get; init; } = string.Empty;

    [Option('c', "colors", HelpText = "Color scheme (Random/Gradient)")]
    public ColorSchemeType ColorScheme { get; init; } = ColorSchemeType.Random;

    [Option("boring-words", HelpText = "Path to file with boring words")]
    public string? BoringWordsFile { get; init; }
    
    [Option("min-font", Default = 10, HelpText = "Minimum font size")]
    public int MinFontSize { get; init; }
    
    [Option("max-font", Default = 50, HelpText = "Maximum font size")]
    public int MaxFontSize { get; init; }
    
    [Option("algorithm", Default = TagCloudAlgorithmType.Spiral, HelpText =
        "Algorithm: Spiral | Tight")]
    public TagCloudAlgorithmType Algorithm { get; init; } = TagCloudAlgorithmType.Spiral;
}
