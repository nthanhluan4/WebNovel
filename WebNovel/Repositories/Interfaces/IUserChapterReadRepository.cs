using WebNovel.Models;

namespace WebNovel.Repositories.Interfaces
{
    public interface IUserChapterReadRepository
    {
        Task<bool> IsChapterReadAsync(string userId, string chapterId);
        Task AddAsync(UserChapterRead entity);
        Task<List<UserChapterRead>> GetReadChaptersByUserAsync(string userId, int storyId);
    }

}
