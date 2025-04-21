using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Repositories.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    public class StoryRepository : IStoryRepository
    {
        private readonly ApplicationDbContext _context;

        public StoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Story>> GetAllAsync() =>
            await _context.Stories.AsNoTracking().ToListAsync();

        public async Task<Story?> GetByIdAsync(int id) =>
            await _context.Stories.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

        public async Task<Story?> GetBySlugAsync(string slug) =>
            await _context.Stories.AsNoTracking().FirstOrDefaultAsync(s => s.Slug == slug);

        public async Task<List<Story>> GetByGenreAsync(int genreId)
        {
            var idStr = "," + genreId.ToString() + ",";
            return await _context.Stories
                .Where(s => ("," + s.GenreIds + ",").Contains(idStr))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Story>> GetByAuthorIdAsync(int authorId) =>
            await _context.Stories
                .Where(s => s.AuthorId == authorId)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Story>> GetByContributorIdAsync(int contributorId) =>
            await _context.Stories
                .Where(s => s.ContributorId == contributorId)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Story>> SearchAsync(string keyword) =>
            await _context.Stories
                .Where(s => s.Title.Contains(keyword) || s.AuthorName.Contains(keyword))
                .AsNoTracking()
                .ToListAsync();

        public async Task AddAsync(Story story) =>
            await _context.Stories.AddAsync(story);

        public Task UpdateAsync(Story story)
        {
            _context.Stories.Update(story);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var story = await _context.Stories.FindAsync(id);
            if (story != null)
            {
                _context.Stories.Remove(story);
            }
        }


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


        public async Task<bool> SaveChangesAsync() =>
            (await _context.SaveChangesAsync()) > 0;
    }
}
