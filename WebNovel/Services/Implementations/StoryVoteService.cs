using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Models.Dtos;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Interfaces;

namespace WebNovel.Services.Implementations
{
    public class StoryVoteService : IStoryVoteService
    {
        private readonly IStoryVoteRepository _voteRepo;
        private readonly ApplicationDbContext _context;

        public StoryVoteService(IStoryVoteRepository voteRepo, ApplicationDbContext context)
        {
            _voteRepo = voteRepo;
            _context = context;
        }

        public async Task<ServiceResponse<StoryVote>> VoteAsync(string userId, int storyId, long voteToAdd)
        {
            if (voteToAdd <= 0)
                return ServiceResponse<StoryVote>.Fail("Số phiếu phải lớn hơn 0.");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return ServiceResponse<StoryVote>.Fail("Không tìm thấy user.");

            var story = await _context.Stories.FindAsync(storyId);
            if (story == null)
                return ServiceResponse<StoryVote>.Fail("Không tìm thấy truyện.");

            var vote = await _voteRepo.GetByUserAndStoryAsync(userId, storyId);

            if (vote == null)
            {
                // Lần đầu vote ➝ 1 phiếu miễn phí
                vote = new StoryVote
                {
                    UserId = userId,
                    StoryId = storyId,
                    VoteCount = 1
                };

                story.TotalVotes += 1;

                if (voteToAdd > 1)
                {
                    var needTickets = voteToAdd - 1;
                    if (user.VoteTickets < needTickets)
                        return ServiceResponse<StoryVote>.Fail("Bạn không đủ vé để đề cử.");

                    user.VoteTickets -= needTickets;
                    vote.VoteCount += needTickets;
                    story.TotalVotes += needTickets;
                }

                await _voteRepo.AddAsync(vote);
            }
            else
            {
                if (user.VoteTickets < voteToAdd)
                    return ServiceResponse<StoryVote>.Fail("Bạn không đủ vé để đề cử.");

                user.VoteTickets -= voteToAdd;
                vote.VoteCount += voteToAdd;
                story.TotalVotes += voteToAdd;

                await _voteRepo.UpdateAsync(vote);
            }

            await _context.SaveChangesAsync();
                    return ServiceResponse<StoryVote>.Ok(vote, "Đề cử thành công.");
        }
    }

}
