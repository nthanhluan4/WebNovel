using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebNovel.Exceptions;
using WebNovel.Models.Dtos;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Interfaces;
using WebNovel.Utils;

namespace WebNovel.Services.Implementations
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IBaseRepository<T> _repo;
        private readonly IModel _model;

        public BaseService(IBaseRepository<T> repo, IModel model)
        {
            _repo = repo;
            _model = model;
        }

        public async Task<DataSourceResult> GetAllDataSourceAsync(DataSourceRequest request)
        {
            return await _repo.Query().ToDataSourceResultAsync(request);
        }

        public async Task<List<T>> GetDropdownDataAsync()
        {
            return await _repo.Query().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
            //return item == null
            //    ? ServiceResponse<T>.Fail("Item not found")
            //    : ServiceResponse<T>.Ok(item);
        }

        public async Task<ServiceResponse<T>> CreateAsync(T entity)
        {
            await EnsureNoDuplicate(entity);
            await _repo.AddAsync(entity);
            return ServiceResponse<T>.Ok(entity, "Created successfully");
        }

        public async Task<ServiceResponse<T>> UpdateAsync(int id, T entity)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return ServiceResponse<T>.Fail("Item not found");
            await EnsureNoDuplicate(entity, id);
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

        private async Task EnsureNoDuplicate(T entity, int? excludeId = null)
        {
            var entityType = typeof(T);
            var uniqueFields = _model.FindEntityType(entityType)?
                .GetIndexes()
                .Where(i => i.IsUnique)
                .SelectMany(i => i.Properties.Select(p => p.Name))
                .Distinct()
                .ToList();

            if (uniqueFields == null || !uniqueFields.Any())
                return;

            foreach (var field in uniqueFields)
            {
                var prop = entityType.GetProperty(field);
                if (prop == null) continue;

                var value = prop.GetValue(entity);
                if (value == null) continue;

                var exists = await _repo.ExistsAsync(x =>
                    EF.Property<object>(x, field).Equals(value) &&
                    (excludeId == null || EF.Property<int>(x, "Id") != excludeId.Value));

                if (exists)
                    throw new DuplicateDataException($"Dữ liệu [{field}: '{value}'] đã có trong hệ thống.");
            }
        }
    }

}
