using TagsCloudContainer.Core.Infrastructure.Coloring;
using TagsCloudContainer.Core.Infrastructure.Layout;

namespace TagsCloudContainer.Core.Models;
public class TagCloudSettings
{
    public int Width { get; set; } = 800;
    public int Height { get; set; } = 600;
    public string FontFamily { get; set; } = "Arial";
    public ColorSchemeType ColorScheme { get; init; } = ColorSchemeType.Random;
    public TagCloudAlgorithmType Algorithm { get; init; } = TagCloudAlgorithmType.Spiral;
    public int MinFontSize { get; init; } = 10;
    public int MaxFontSize { get; init; } = 50;
    public string? BoringWordsPath { get; init; } = string.Empty;
}
