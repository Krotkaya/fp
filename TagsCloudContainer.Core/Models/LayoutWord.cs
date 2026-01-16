using SkiaSharp;

namespace TagsCloudContainer.Core.Models;
public class LayoutWord
{
    public WordFrequency WordFrequency { get; }
    public SKTypeface Typeface { get; }
    public float FontSize { get; }
    public SKColor Color { get; }
    public SKRect Rectangle { get; }
    
    public LayoutWord(WordFrequency wordFrequency, SKTypeface typeface, 
        float fontSize, SKColor color, SKRect rectangle)
    {
        WordFrequency = wordFrequency;
        Typeface = typeface;
        FontSize = fontSize;
        Color = color;
        Rectangle = rectangle;
    }
}