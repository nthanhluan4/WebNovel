using Microsoft.EntityFrameworkCore;
using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Repositories.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> GetAllAsync() =>
            await _context.Authors.AsNoTracking().ToListAsync();

        public async Task<Author?> GetByIdAsync(int id) =>
            await _context.Authors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

        public async Task<Author?> GetBySlugAsync(string slug) =>
            await _context.Authors.AsNoTracking().FirstOrDefaultAsync(a => a.Slug == slug);

        public async Task<bool> ExistsByNameAsync(string name) =>
            await _context.Authors.AnyAsync(a => a.Name.ToLower() == name.ToLower());

        public async Task AddAsync(Author author) => await _context.Authors.AddAsync(author);

        public Task UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
            }
        }

        public async Task<bool> SaveChangesAsync() =>
            (await _context.SaveChangesAsync()) > 0;
    }
}
