using Microsoft.EntityFrameworkCore;
using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Repositories.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    public class UserChapterReadRepository : IUserChapterReadRepository
    {
        private readonly ApplicationDbContext _context;

        public UserChapterReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsChapterReadAsync(string userId, string chapterId)
        {
            return await _context.UserChapterReads
                .AnyAsync(x => x.UserId == userId && x.ChapterId == chapterId);
        }

        public async Task AddAsync(UserChapterRead entity)
        {
            _context.UserChapterReads.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserChapterRead>> GetReadChaptersByUserAsync(string userId, int storyId)
        {
            return await _context.UserChapterReads
                .Where(x => x.UserId == userId && x.StoryId == storyId)
                .OrderBy(x => x.ReadAt)
                .ToListAsync();
        }
    }

}
