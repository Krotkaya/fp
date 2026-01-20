using ResultOf;
using TagsCloudContainer.Core.Infrastructure.Coloring;
using TagsCloudContainer.Core.Infrastructure.Layout;

namespace TagsCloudContainer.Core.Models;
public sealed class TagCloudSettings
{
    public int Width { get; }
    public int Height { get; }
    public string FontFamily { get; }
    public ColorSchemeType ColorScheme { get; }
    public TagCloudAlgorithmType Algorithm { get; }
    public int MinFontSize { get; }
    public int MaxFontSize { get; }
    public string? BoringWordsPath { get; }

    private TagCloudSettings(
        int width,
        int height,
        string fontFamily,
        ColorSchemeType colorScheme,
        TagCloudAlgorithmType algorithm,
        int minFontSize,
        int maxFontSize,
        string? boringWordsPath)
    {
        Width = width;
        Height = height;
        FontFamily = fontFamily;
        ColorScheme = colorScheme;
        Algorithm = algorithm;
        MinFontSize = minFontSize;
        MaxFontSize = maxFontSize;
        BoringWordsPath = boringWordsPath;
    }

    public static Result<TagCloudSettings> Create(TagCloudSettingsDto? data)
    {
        if (data == null)
            return Result.Fail<TagCloudSettings>("Settings are not specified");
        
        var boringWordsPath = string.IsNullOrWhiteSpace(data.BoringWordsPath)
            ? null
            : data.BoringWordsPath;

        var settings = new TagCloudSettings(
            data.Width,
            data.Height,
            data.FontFamily,
            data.ColorScheme,
            data.Algorithm,
            data.MinFontSize,
            data.MaxFontSize,
            boringWordsPath);
        return new TagCloudSettingsValidator().Validate(settings);
    }
}
