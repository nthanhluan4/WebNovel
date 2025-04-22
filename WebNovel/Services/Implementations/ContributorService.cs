using WebNovel.Exceptions;
using WebNovel.Models;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Interfaces;

namespace WebNovel.Services.Implementations
{
    public class ContributorService : IContributorService
    {
        private readonly IContributorRepository _repository;

        public ContributorService(IContributorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Contributor>> GetAllApprovedAsync() => await _repository.GetAllApprovedAsync();
        public async Task<List<Contributor>> GetAllPendingAsync() => await _repository.GetAllPendingAsync();
        public async Task<Contributor?> GetByUserIdAsync(string userId) => await _repository.GetByUserIdAsync(userId);
        public async Task<Contributor?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<Contributor?> GetBySlugAsync(string slug) => await _repository.GetBySlugAsync(slug);

        public async Task<bool> RequestToBecomeAsync(Contributor contributor)
        {
            var existing = await _repository.GetByUserIdAsync(contributor.CreatedByUserId);
            if (existing != null) 
                //return false;
            throw new DuplicateDataException($"Bạn đã yêu cầu trở thành converter '{contributor.Name}'.");

            contributor.IsApproved = false;
            await _repository.AddAsync(contributor);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> UpdateProfileAsync(Contributor contributor)
        {
            await _repository.UpdateAsync(contributor);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var contributor = await _repository.GetByIdAsync(id);
            if (contributor == null) return false;
            contributor.IsApproved = true;
            await _repository.UpdateAsync(contributor);
            return await _repository.SaveChangesAsync();
        }
    }
}
