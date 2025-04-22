using WebNovel.Models;

namespace WebNovel.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetAllAsync();
        Task<Rating?> GetByIdAsync(int id);
        Task AddAsync(Rating rating);
        Task UpdateAsync(Rating rating);
        Task DeleteAsync(int id);
        Task<IEnumerable<Rating>> GetByStoryIdAsync(int storyId);
        Task<IEnumerable<Rating>> GetByChapterIdAsync(int chapterId);
    }

}
