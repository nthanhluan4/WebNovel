using WebNovel.Models;

namespace WebNovel.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAllAsync();
        Task<Genre?> GetByIdAsync(int id);
        Task<Genre?> GetBySlugAsync(string slug);
        Task<bool> ExistsByNameAsync(string name);
        Task AddAsync(Genre genre);
        Task UpdateAsync(Genre genre);
        Task DeleteAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
