using WebNovel.Models;

namespace WebNovel.Repositories.Interfaces
{
    public interface ILookupRepository
    {
        Task<Dictionary<int, string>> GetAllGenresAsync();
        Task<Dictionary<int, Genre>> GetGenresAsync();
        Task<Dictionary<int, string>> GetAllTagsAsync();
        Task<Dictionary<int, Tag>> GetTagsAsync();
    }

}
