using Microsoft.EntityFrameworkCore;
using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Repositories.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    public class ContributorRepository : IContributorRepository
    {
        private readonly ApplicationDbContext _context;

        public ContributorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Contributor>> GetAllApprovedAsync() =>
            await _context.Contributors.Where(c => c.IsApproved).AsNoTracking().ToListAsync();

        public async Task<List<Contributor>> GetAllPendingAsync() =>
            await _context.Contributors.Where(c => !c.IsApproved).AsNoTracking().ToListAsync();

        public async Task<Contributor?> GetByIdAsync(int id) =>
            await _context.Contributors.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Contributor?> GetByUserIdAsync(string userId) =>
            await _context.Contributors.FirstOrDefaultAsync(c => c.CreatedByUserId == userId);

        public async Task<Contributor?> GetBySlugAsync(string slug) =>
            await _context.Contributors.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == slug);

        public async Task AddAsync(Contributor contributor) => await _context.Contributors.AddAsync(contributor);

        public Task UpdateAsync(Contributor contributor)
        {
            _context.Contributors.Update(contributor);
            return Task.CompletedTask;
        }

        public async Task<bool> SaveChangesAsync() =>
            (await _context.SaveChangesAsync()) > 0;
    }
}
