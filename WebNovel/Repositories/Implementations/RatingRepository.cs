using Microsoft.EntityFrameworkCore;
using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Repositories.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _context;
        public RatingRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Rating>> GetAllAsync() => await _context.Ratings.AsNoTracking().ToListAsync();
        public async Task<Rating?> GetByIdAsync(int id) => await _context.Ratings.FindAsync(id);
        public async Task AddAsync(Rating rating) { _context.Ratings.Add(rating); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(Rating rating) { _context.Ratings.Update(rating); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating != null) { _context.Ratings.Remove(rating); await _context.SaveChangesAsync(); }
        }

        public async Task<IEnumerable<Rating>> GetByStoryIdAsync(int storyId)
            => await _context.Ratings.Where(r => r.StoryId == storyId).ToListAsync();

        public async Task<IEnumerable<Rating>> GetByChapterIdAsync(int chapterId)
            => await _context.Ratings.Where(r => r.ChapterId == chapterId).ToListAsync();
    }

}
