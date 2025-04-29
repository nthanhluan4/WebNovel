using Microsoft.EntityFrameworkCore;
using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Repositories.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    public class StoryVoteRepository : IStoryVoteRepository
    {
        private readonly ApplicationDbContext _context;

        public StoryVoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StoryVote?> GetByUserAndStoryAsync(string userId, int storyId)
        {
            return await _context.StoryVotes
                .FirstOrDefaultAsync(v => v.UserId == userId && v.StoryId == storyId);
        }

        public async Task AddAsync(StoryVote vote)
        {
            _context.StoryVotes.Add(vote);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StoryVote vote)
        {
            _context.StoryVotes.Update(vote);
            await _context.SaveChangesAsync();
        }
    }

}
