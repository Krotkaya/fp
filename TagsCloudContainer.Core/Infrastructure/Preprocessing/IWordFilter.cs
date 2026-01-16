namespace TagsCloudContainer.Core.Infrastructure.Preprocessing;
public interface IWordFilter
{
    bool ShouldExclude(string word);
}