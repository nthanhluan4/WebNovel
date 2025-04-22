using WebNovel.Models;

namespace WebNovel.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag?> GetTagByIdAsync(int id);
        Task<Tag?> GetTagBySlugAsync(string slug);
        Task<Tag> CreateTagAsync(Tag tag);
        Task<Tag?> UpdateTagAsync(int id, Tag tag);
        Task<bool> DeleteTagAsync(int id);
    }

}
