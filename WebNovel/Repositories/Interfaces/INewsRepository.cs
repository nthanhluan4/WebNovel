using WebNovel.Models;

namespace WebNovel.Repositories.Interfaces
{
    public interface INewsRepository : ISlugRepository<News>
    {
        Task<News?> GetBySlugAsync(string slug);
        Task<List<News>> GetPinnedAsync();
    }

}
