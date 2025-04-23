using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using WebNovel.Models.Dtos;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Interfaces;

namespace WebNovel.Services.Implementations
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IBaseRepository<T> _repo;

        public BaseService(IBaseRepository<T> repo)
        {
            _repo = repo;
        }

        public async Task<DataSourceResult> GetAllDataSourceAsync(DataSourceRequest request)
        {
            return await _repo.Query().ToDataSourceResultAsync(request);
        }

        public async Task<List<T>> GetDropdownDataAsync()
        {
            return await _repo.Query().ToListAsync();
        }

        public async Task<ServiceResponse<T>> GetByIdAsync(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            return item == null
                ? ServiceResponse<T>.Fail("Item not found")
                : ServiceResponse<T>.Ok(item);
        }

        public async Task<ServiceResponse<T>> CreateAsync(T entity)
        {
            await _repo.AddAsync(entity);
            return ServiceResponse<T>.Ok(entity, "Created successfully");
        }

        public async Task<ServiceResponse<T>> UpdateAsync(int id, T entity)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return ServiceResponse<T>.Fail("Item not found");
            await _repo.UpdateAsync(entity);
            return ServiceResponse<T>.Ok(entity, "Updated successfully");
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return ServiceResponse<bool>.Fail("Item not found");
            await _repo.DeleteAsync(id);
            return ServiceResponse<bool>.Ok(true, "Deleted successfully");
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _repo.Query().ToListAsync();
        }
    }

}
