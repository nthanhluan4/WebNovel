using WebNovel.Models;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Interfaces;

namespace WebNovel.Services.Implementations
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _repo;
        public RatingService(IRatingRepository repo) => _repo = repo;

        public Task<IEnumerable<Rating>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Rating?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public async Task<Rating> CreateAsync(Rating rating) { await _repo.AddAsync(rating); return rating; }
        public async Task<Rating?> UpdateAsync(int id, Rating rating)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;
            existing.Stars = rating.Stars;
            existing.Comment = rating.Comment;
            await _repo.UpdateAsync(existing);
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }

        public Task<IEnumerable<Rating>> GetByStoryIdAsync(int storyId) => _repo.GetByStoryIdAsync(storyId);
        public Task<IEnumerable<Rating>> GetByChapterIdAsync(int chapterId) => _repo.GetByChapterIdAsync(chapterId);
    }

}
