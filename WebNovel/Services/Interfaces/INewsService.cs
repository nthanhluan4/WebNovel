using WebNovel.Models;

namespace WebNovel.Services.Interfaces
{
    public interface INewsService : ISlugService<News>
    {
        Task<List<News>> GetPinnedAsync();
    }

}
