using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NSubstitute;
using SkiaSharp;
using TagsCloudContainer.Core.Infrastructure.Coloring;
using TagsCloudContainer.Core.Infrastructure.Layout;
using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Tests;

[TestFixture]
public class SpiralTagCloudAlgorithmTests
{
    [Test]
    public void Arrange_ShouldPlaceAllWordsWithoutIntersections()
    {
        const float fontSize = 20f;
        var fontCalculator = Substitute.For<IFontSizeCalculator>();
        fontCalculator.Calculate(Arg.Any<int>(), Arg.Any<int>()).Returns(fontSize);

        var colorScheme = Substitute.For<IColorScheme>();
        colorScheme.GetColor(Arg.Any<int>()).Returns(SKColors.Black);

        var algorithm = new SpiralTagCloudAlgorithm(fontCalculator, colorScheme);
        var options = new LayoutOptions();

        var frequencies = new[]
        {
            new WordFrequency("one", 5),
            new WordFrequency("two", 3),
            new WordFrequency("three", 1)
        };

        var layoutWords = algorithm.Arrange(frequencies, options).ToArray();

        layoutWords.Should().HaveCount(frequencies.Length);

        for (var i = 0; i < layoutWords.Length; i++)
        {
            for (var j = i + 1; j < layoutWords.Length; j++)
            {
                layoutWords[i].Rectangle.IntersectsWith(layoutWords[j].Rectangle)
                    .Should().BeFalse($"rectangles {i} and {j} should not overlap");
            }
        }
    }

    [Test]
    public void Arrange_ShouldPlaceFirstWordAroundCenter()
    {
        var fontCalculator = Substitute.For<IFontSizeCalculator>();
        fontCalculator.Calculate(Arg.Any<int>(), Arg.Any<int>()).Returns(18f);

        var colorScheme = Substitute.For<IColorScheme>();
        colorScheme.GetColor(Arg.Any<int>()).Returns(SKColors.Black);

        var algorithm = new SpiralTagCloudAlgorithm(fontCalculator, colorScheme);
        var options = new LayoutOptions();

        var frequencies = new[] { new WordFrequency("center", 10) };

        var layoutWords = algorithm.Arrange(frequencies, options).ToArray();

        layoutWords.Should().ContainSingle();
        layoutWords[0].Rectangle.Contains(options.Center).Should().BeTrue();
    }
}
