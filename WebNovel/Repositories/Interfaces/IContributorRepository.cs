using WebNovel.Models;

namespace WebNovel.Repositories.Interfaces
{
    public interface IContributorRepository
    {
        Task<List<Contributor>> GetAllApprovedAsync();
        Task<List<Contributor>> GetAllPendingAsync();
        Task<Contributor?> GetByIdAsync(int id);
        Task<Contributor?> GetByUserIdAsync(string userId);
        Task<Contributor?> GetBySlugAsync(string slug);
        Task AddAsync(Contributor contributor);
        Task UpdateAsync(Contributor contributor);
        Task<bool> SaveChangesAsync();
    }
}
