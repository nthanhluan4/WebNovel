using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebNovel.Data;
using WebNovel.Repositories.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> Query() => _dbSet.AsQueryable();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            //_dbSet.Update(entity);

            var keyProp = typeof(T).GetProperty("Id");
            if (keyProp == null) throw new Exception("Entity phải có trường Id");

            var id = keyProp.GetValue(entity);
            var existing = await _dbSet.FindAsync(id);

            if (existing == null)
                throw new Exception($"Không tìm thấy bản ghi với Id = {id}");

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }

}
