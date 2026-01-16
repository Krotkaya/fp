namespace TagsCloudContainer.Core.Infrastructure.Layout;
public class LinearFontSizeCalculator(float minFontSize = 10, float maxFontSize = 50) 
    : IFontSizeCalculator
{
    public float Calculate(int frequency, int maxFrequency)
    {
        if (maxFrequency <= 0)
            return minFontSize;

        var ratio = (float)frequency / maxFrequency;
        var fontSize = minFontSize + (maxFontSize - minFontSize) * ratio;
        
        return Math.Max(minFontSize, Math.Min(maxFontSize, fontSize));
    }
}
