using WebNovel.Models.Dtos;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Interfaces;

namespace WebNovel.Services.Implementations
{
    public class SlugService<T> : BaseService<T>, ISlugService<T> where T : class, ISlugEntity
    {
        private readonly ISlugRepository<T> _slugRepo;

        public SlugService(ISlugRepository<T> slugRepo) : base(slugRepo)
        {
            _slugRepo = slugRepo;
        }

        public async Task<ServiceResponse<T>> GetBySlugAsync(string slug)
        {
            var entity = await _slugRepo.GetBySlugAsync(slug);
            return entity == null
                ? ServiceResponse<T>.Fail("Not found")
                : ServiceResponse<T>.Ok(entity);
        }
    }

}
