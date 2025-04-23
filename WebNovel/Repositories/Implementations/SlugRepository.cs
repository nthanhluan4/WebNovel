using Microsoft.EntityFrameworkCore;
using WebNovel.Data;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    public class SlugRepository<T> : BaseRepository<T>, ISlugRepository<T> where T : class, ISlugEntity
    {
        public SlugRepository(ApplicationDbContext context) : base(context) { }

        public async Task<T?> GetBySlugAsync(string slug)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Slug == slug);
        }
    }

}
