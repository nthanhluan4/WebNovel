using Microsoft.EntityFrameworkCore;
using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Repositories.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    
    public class NewsRepository : SlugRepository<News>, INewsRepository
    {
        private readonly ApplicationDbContext _context;
        public NewsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<News?> GetBySlugAsync(string slug)
        {
            return await _context.News.FirstOrDefaultAsync(x => x.Slug == slug && x.IsPublished);
        }

        public async Task<List<News>> GetPinnedAsync()
        {
            return await _context.News
                .Where(x => x.IsPinned && x.IsPublished)
                .OrderBy(x => x.PinnedPosition)
                .ThenByDescending(x => x.PinnedAt)
                .ToListAsync();
        }
    }

}
