using ResultOf;
namespace TagsCloudContainer.Core.Infrastructure.Preprocessing;

public class DefaultBoringWordsProvider : IBoringWordsProvider
{
    public Result<string[]> GetWords() => Result.Ok(DefaultBoringWords);

    private static readonly string[] DefaultBoringWords =
    [
        "и", "в", "не", "на", "я", "он", "с", "что", "а", "по",
        "это", "как", "но", "они", "к", "у", "же", "вы", "за",
        "бы", "о", "из", "от", "то", "все", "его", "до", "для",
        "мы", "при", "так", "та", "их", "чем", "или", "быть",
        "вот", "от", "под", "ну", "ни", "да", "ли", "если", "еще"
    ];
}