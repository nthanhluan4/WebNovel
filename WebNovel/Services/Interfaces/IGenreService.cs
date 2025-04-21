using WebNovel.Models;

namespace WebNovel.Services.Interfaces
{
    public interface IGenreService
    {
        Task<List<Genre>> GetAllAsync();
        Task<Genre?> GetByIdAsync(int id);
        Task<Genre?> GetBySlugAsync(string slug);
        Task<bool> CreateAsync(Genre genre);
        Task<bool> UpdateAsync(Genre genre);
        Task<bool> DeleteAsync(int id);
    }
}
