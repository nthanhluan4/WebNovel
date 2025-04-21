using Microsoft.EntityFrameworkCore;
using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Repositories.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Genre>> GetAllAsync() =>
            await _context.Genres.AsNoTracking().ToListAsync();

        public async Task<Genre?> GetByIdAsync(int id) =>
            await _context.Genres.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);

        public async Task<Genre?> GetBySlugAsync(string slug) =>
            await _context.Genres.AsNoTracking().FirstOrDefaultAsync(g => g.Slug == slug);

        public async Task<bool> ExistsByNameAsync(string name) =>
            await _context.Genres.AnyAsync(g => g.Name.ToLower() == name.ToLower());

        public async Task AddAsync(Genre genre) => await _context.Genres.AddAsync(genre);

        public Task UpdateAsync(Genre genre)
        {
            _context.Genres.Update(genre);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null) _context.Genres.Remove(genre);
        }

        public async Task<bool> SaveChangesAsync() =>
            (await _context.SaveChangesAsync()) > 0;
    }
}
