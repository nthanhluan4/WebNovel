using WebNovel.Models;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Interfaces;

namespace WebNovel.Services.Implementations
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _repository;

        public GenreService(IGenreRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Genre>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Genre?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<Genre?> GetBySlugAsync(string slug) => await _repository.GetBySlugAsync(slug);

        public async Task<bool> CreateAsync(Genre genre)
        {
            if (await _repository.ExistsByNameAsync(genre.Name)) return false;
            await _repository.AddAsync(genre);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Genre genre)
        {
            await _repository.UpdateAsync(genre);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return await _repository.SaveChangesAsync();
        }
    }
}
