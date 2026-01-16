using System.IO;
using FluentAssertions;
using NUnit.Framework;
using SkiaSharp;
using TagsCloudContainer.Core.Infrastructure.Rendering;
using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Tests;

[TestFixture]
public class PngRendererTests
{
    [Test]
    public void SaveToFile_ShouldCreateNonEmptyPng()
    {
        var renderer = new PngRenderer();
        var options = new LayoutOptions();
        var typeface = SKTypeface.FromFamilyName(options.FontFamily);

        var layoutWords = new[]
        {
            new LayoutWord(new WordFrequency("hello", 1), typeface, 24,
                SKColors.Black, new SKRect(10, 10, 70, 40)),
            new LayoutWord(new WordFrequency("world", 1), typeface, 24,
                SKColors.Blue, new SKRect(80, 10, 150, 40))
        };

        using var image = renderer.Render(layoutWords, options);
        var tempPath = Path.GetTempFileName();

        try
        {
            renderer.SaveToFile(image, tempPath);

            File.Exists(tempPath).Should().BeTrue();
            new FileInfo(tempPath).Length.Should().BeGreaterThan(0);
        }
        finally
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }
}
