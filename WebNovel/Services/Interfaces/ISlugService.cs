using WebNovel.Models.Dtos;

namespace WebNovel.Services.Interfaces
{
    public interface ISlugService<T> : IBaseService<T> where T : class
    {
        Task<ServiceResponse<T>> GetBySlugAsync(string slug);
    }

}
