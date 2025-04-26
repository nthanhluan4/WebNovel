using WebNovel.Models;

namespace WebNovel.Repositories.Interfaces
{
    public interface IChapterRepository
    {
        Task<List<Chapter>> GetAllAsync();
        Task<List<Chapter>> GetByStoryIdAsync(int storyId);
        Task<Chapter?> GetByIdAsync(string id);
        Task<Chapter?> GetByOrderAsync(int storyId, int order);
        Task AddAsync(Chapter chapter);
        Task UpdateAsync(Chapter chapter);
        Task DeleteAsync(string id);

        Task<int> CountChaptersAsync(int storyId);
        Task<long> SumWordsAsync(int storyId);

        Task<bool> SaveChangesAsync();
    }
}
