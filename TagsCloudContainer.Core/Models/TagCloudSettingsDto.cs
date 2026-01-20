using TagsCloudContainer.Core.Infrastructure.Coloring;
using TagsCloudContainer.Core.Infrastructure.Layout;

namespace TagsCloudContainer.Core.Models;
public class TagCloudSettingsDto
{
    public int Width { get; set; } = 800;
    public int Height { get; set; } = 600;
    public string FontFamily { get; set; } = "Arial";
    public ColorSchemeType ColorScheme { get; set; } = ColorSchemeType.Random;
    public TagCloudAlgorithmType Algorithm { get; set; } = TagCloudAlgorithmType.Spiral;
    public int MinFontSize { get; set; } = 10;
    public int MaxFontSize { get; set; } = 50;
    public string? BoringWordsPath { get; set; } = string.Empty;
}
