using Microsoft.EntityFrameworkCore;
using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Repositories.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    public class LookupRepository : ILookupRepository
    {
        private readonly ApplicationDbContext _context;

        public LookupRepository(ApplicationDbContext context)
            => _context = context;

        public async Task<Dictionary<int, string>> GetAllGenresAsync()
            => await _context.Genres
                       .AsNoTracking()
                       .ToDictionaryAsync(g => g.Id, g => g.Name);

        public async Task<Dictionary<int, Genre>> GetGenresAsync()
          => await _context.Genres
                     .AsNoTracking()
                     .ToDictionaryAsync(g => g.Id, g => g);

        public async Task<Dictionary<int, string>> GetAllTagsAsync()
            => await _context.Tags
                       .AsNoTracking()
                       .ToDictionaryAsync(t => t.Id, t => t.Name);
        public async Task<Dictionary<int, Tag>> GetTagsAsync()
           => await _context.Tags
                      .AsNoTracking()
                      .ToDictionaryAsync(t => t.Id, t => t);
    }

}
