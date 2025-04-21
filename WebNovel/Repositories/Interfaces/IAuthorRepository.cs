using WebNovel.Models;

namespace WebNovel.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(int id);
        Task<Author?> GetBySlugAsync(string slug);
        Task<bool> ExistsByNameAsync(string name);
        Task AddAsync(Author author);
        Task UpdateAsync(Author author);
        Task DeleteAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
