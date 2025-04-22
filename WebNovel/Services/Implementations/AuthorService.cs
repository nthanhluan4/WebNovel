using WebNovel.Exceptions;
using WebNovel.Models;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Interfaces;

namespace WebNovel.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repository;

        public AuthorService(IAuthorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Author>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<Author?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<Author?> GetBySlugAsync(string slug) => await _repository.GetBySlugAsync(slug);

        public async Task<bool> CreateAsync(Author author)
        {
            if (await _repository.ExistsByNameAsync(author.Name))
                //return false;
                throw new DuplicateDataException($"Tác giả '{author.Name}' đã tồn tại.");
            await _repository.AddAsync(author);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Author author)
        {
            await _repository.UpdateAsync(author);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return await _repository.SaveChangesAsync();
        }
    }
}