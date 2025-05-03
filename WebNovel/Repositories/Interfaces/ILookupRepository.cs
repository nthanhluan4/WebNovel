namespace WebNovel.Repositories.Interfaces
{
    public interface ILookupRepository
    {
        Task<Dictionary<int, string>> GetAllGenresAsync();
        Task<Dictionary<int, string>> GetAllTagsAsync();
    }

}
