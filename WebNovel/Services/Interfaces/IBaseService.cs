using Kendo.Mvc.UI;
using WebNovel.Models.Dtos;

namespace WebNovel.Services.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        Task<List<T>> GetAllAsync(); 
        Task<DataSourceResult> GetAllDataSourceAsync(DataSourceRequest request);
        Task<List<T>> GetDropdownDataAsync();
        Task<ServiceResponse<T>> GetByIdAsync(int id);
        Task<ServiceResponse<T>> CreateAsync(T entity);
        Task<ServiceResponse<T>> UpdateAsync(int id, T entity);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }


}
