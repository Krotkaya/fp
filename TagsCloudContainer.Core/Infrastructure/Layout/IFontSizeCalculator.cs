namespace TagsCloudContainer.Core.Infrastructure.Layout;

public interface IFontSizeCalculator
{
    float Calculate(int frequency, int maxFrequency);
}