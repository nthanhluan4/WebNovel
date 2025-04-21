using Microsoft.EntityFrameworkCore;
using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Repositories.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly ApplicationDbContext _context;

        public ChapterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Chapter>> GetByStoryIdAsync(int storyId) =>
            await _context.Chapters.Where(c => c.StoryId == storyId).OrderBy(c => c.Order).ToListAsync();

        public async Task<Chapter?> GetByIdAsync(string id) =>
            await _context.Chapters.FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Chapter?> GetByOrderAsync(int storyId, int order) =>
            await _context.Chapters.FirstOrDefaultAsync(c => c.StoryId == storyId && c.Order == order);

        public async Task AddAsync(Chapter chapter) => await _context.Chapters.AddAsync(chapter);

        public Task UpdateAsync(Chapter chapter)
        {
            _context.Chapters.Update(chapter);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(string id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter != null) _context.Chapters.Remove(chapter);
        }

        public async Task<bool> SaveChangesAsync() => (await _context.SaveChangesAsync()) > 0;

        public async Task<int> CountChaptersAsync(int storyId)
        {
            return await _context.Chapters.Where(c => c.StoryId == storyId).CountAsync();
        }

        public async Task<long> SumWordsAsync(int storyId)
        {
            return await _context.Chapters
                .Where(c => c.StoryId == storyId)
                .SumAsync(c => (long?)c.WordCount) ?? 0;
        }
    }
}
