using WebNovel.Models;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Interfaces;

namespace WebNovel.Services.Implementations
{
    public class ChapterReadingService : IChapterReadingService
    {
        private readonly IChapterReadByDateRepository _readByDateRepo;
        private readonly IUserChapterReadRepository _userReadRepo;

        public ChapterReadingService(IChapterReadByDateRepository readByDateRepo, IUserChapterReadRepository userReadRepo)
        {
            _readByDateRepo = readByDateRepo;
            _userReadRepo = userReadRepo;
        }

        public async Task RecordChapterReadAsync(string? userId, int storyId, string chapterId)
        {
            var today = DateTime.UtcNow.Date;

            // 1. Update ChapterReadByDate
            var readByDate = await _readByDateRepo.GetByChapterAndDateAsync(chapterId, today);
            if (readByDate == null)
            {
                await _readByDateRepo.AddAsync(new ChapterReadByDate
                {
                    ChapterId = chapterId,
                    ReadDate = today,
                    ReadCount = 1
                });
            }
            else
            {
                readByDate.ReadCount += 1;
                await _readByDateRepo.UpdateAsync(readByDate);
            }

            // 2. Insert vào UserChapterRead nếu chưa có
            if (!string.IsNullOrEmpty(userId))
            {

                var alreadyRead = await _userReadRepo.IsChapterReadAsync(userId, chapterId);
                if (!alreadyRead)
                {
                    await _userReadRepo.AddAsync(new UserChapterRead
                    {
                        UserId = userId,
                        StoryId = storyId,
                        ChapterId = chapterId
                    });
                }
            }
        }

        public async Task<List<string>> GetUserReadChapterIdsAsync(string userId, int storyId)
        {
            var records = await _userReadRepo.GetReadChaptersByUserAsync(userId, storyId);
            return records.Select(x => x.ChapterId).ToList();
        }
    }

}
