using WebNovel.Models;

namespace WebNovel.Repositories.Interfaces
{
    public interface IStoryVoteRepository
    {
        Task<StoryVote?> GetByUserAndStoryAsync(string userId, int storyId);
        Task AddAsync(StoryVote vote);
        Task UpdateAsync(StoryVote vote);
    }

}
