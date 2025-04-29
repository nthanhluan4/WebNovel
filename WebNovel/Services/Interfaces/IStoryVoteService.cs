using WebNovel.Models;
using WebNovel.Models.Dtos;

namespace WebNovel.Services.Interfaces
{
    public interface IStoryVoteService
    {
        Task<ServiceResponse<StoryVote>> VoteAsync(string userId, int storyId, long voteToAdd);
    }

}
