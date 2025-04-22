using WebNovel.Models;

namespace WebNovel.Services.Interfaces
{
    public interface IRatingService
    {
        Task<IEnumerable<Rating>> GetAllAsync();
        Task<Rating?> GetByIdAsync(int id);
        Task<Rating> CreateAsync(Rating rating);
        Task<Rating?> UpdateAsync(int id, Rating rating);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Rating>> GetByStoryIdAsync(int storyId);
        Task<IEnumerable<Rating>> GetByChapterIdAsync(int chapterId);
    }

}
