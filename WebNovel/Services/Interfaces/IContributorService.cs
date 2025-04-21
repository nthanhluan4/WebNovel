using WebNovel.Models;

namespace WebNovel.Services.Interfaces
{
    public interface IContributorService
    {
        Task<List<Contributor>> GetAllApprovedAsync();
        Task<List<Contributor>> GetAllPendingAsync();
        Task<Contributor?> GetByUserIdAsync(string userId);
        Task<Contributor?> GetByIdAsync(int id);
        Task<Contributor?> GetBySlugAsync(string slug);
        Task<bool> RequestToBecomeAsync(Contributor contributor);
        Task<bool> UpdateProfileAsync(Contributor contributor);
        Task<bool> ApproveAsync(int id);
    }
}
