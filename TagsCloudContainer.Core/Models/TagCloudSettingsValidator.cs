using ResultOf;

namespace TagsCloudContainer.Core.Models;
public class TagCloudSettingsValidator
{
    private const int MaxImageSide = 10000;
    private const int MaxFontSize = 500;

    public Result<TagCloudSettings> Validate(TagCloudSettings settings)
    {
        if (settings.Width <= 0 || settings.Height <= 0)
            return Result.Fail<TagCloudSettings>("Invalid image size. Width and height must be positive");

        if (settings.Width > MaxImageSide || settings.Height > MaxImageSide)
            return Result.Fail<TagCloudSettings>(
                $"Invalid image size. Max width/height is {MaxImageSide}");

        if (settings.MinFontSize <= 0 || settings.MaxFontSize <= 0)
            return Result.Fail<TagCloudSettings>("Invalid font size. Min/Max must be positive");

        if (settings.MinFontSize > settings.MaxFontSize)
            return Result.Fail<TagCloudSettings>("Invalid font size range. MinFontSize > MaxFontSize");

        if (settings.MaxFontSize > MaxFontSize)
            return Result.Fail<TagCloudSettings>($"Invalid font size. MaxFontSize exceeds {MaxFontSize}");

        if (string.IsNullOrWhiteSpace(settings.FontFamily))
            return Result.Fail<TagCloudSettings>("Font family is not specified");

        if (!string.IsNullOrWhiteSpace(settings.BoringWordsPath) &&
            !File.Exists(settings.BoringWordsPath))
            return Result.Fail<TagCloudSettings>(
                $"Boring words file not found: {settings.BoringWordsPath}");

        return Result.Ok(settings);
    }
}
