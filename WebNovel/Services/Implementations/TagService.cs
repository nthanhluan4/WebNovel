using WebNovel.Models;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Interfaces;

namespace WebNovel.Services.Implementations
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repository;

        public TagService(ITagRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Tag?> GetTagBySlugAsync(string slug)
        {
            return await _repository.GetBySlugAsync(slug);
        }

        public async Task<Tag> CreateTagAsync(Tag tag)
        {
            await _repository.AddAsync(tag);
            return tag;
        }

        public async Task<Tag?> UpdateTagAsync(int id, Tag tag)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Name = tag.Name;
            existing.Slug = tag.Slug;
            existing.Description = tag.Description;

            await _repository.UpdateAsync(existing);
            return existing;
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }

}
