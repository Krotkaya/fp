namespace TagsCloudContainer.Core.Infrastructure.Preprocessing;
public class BoringWordsFilter : ITextPreprocessor
{
    private readonly IWordFilter[] _filters;

    public BoringWordsFilter(IEnumerable<string>? boringWords)
    {
        var boringWords1 = new HashSet<string>(
            boringWords ?? DefaultBoringWords,
            StringComparer.OrdinalIgnoreCase);
        
        _filters =
        [
            new BoringWordFilter(boringWords1)
        ];
    }

    private static readonly string[] DefaultBoringWords =
    [
        "и", "в", "не", "на", "я", "он", "с", "что", "а", "по",
        "это", "как", "но", "они", "к", "у", "же", "вы", "за",
        "бы", "о", "из", "от", "то", "все", "его", "до", "для",
        "мы", "при", "так", "та", "их", "чем", "или", "быть",
        "вот", "от", "под", "ну", "ни", "да", "ли", "если", "еще"
    ];

    public IReadOnlyList<string> Process(IEnumerable<string> words)
    {
        return words
            .Where(word => _filters.All(filter 
                => !filter.ShouldExclude(word))).ToArray();
    }

    private class BoringWordFilter(HashSet<string> boringWords) : IWordFilter
    {
        public bool ShouldExclude(string word)
        {
            return boringWords.Contains(word);
        }
    }
}