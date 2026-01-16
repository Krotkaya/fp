namespace TagsCloudContainer.Core.Models;
public class WordFrequency
{
    public string Word { get; }
    public int Frequency { get; }
    
    public WordFrequency(string word, int frequency)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(frequency, 0);
        Word = word;
        Frequency = frequency;
    }
}