using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebNovel.Models;
using WebNovel.Repositories.Implementations;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Interfaces;
using WebNovel.Utils;

namespace WebNovel.Services.Implementations
{
    public class ChapterService : IChapterService
    {
        private readonly IChapterRepository _repository;
        private readonly IChapterStorageService _storageService;
        private readonly IStoryService _storyService;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<ChapterService> _logger;

        public ChapterService(IChapterRepository repository,
            IChapterStorageService storageService,
            IStoryService storyService,
            IBackgroundTaskQueue taskQueue,
            ILogger<ChapterService> logger)
        {
            _repository = repository;
            _storageService = storageService;
            _storyService = storyService;
            _taskQueue = taskQueue;
            _logger = logger;
        }

        public async Task<List<Chapter>> GetByStoryIdAsync(int storyId) => await _repository.GetByStoryIdAsync(storyId);
        public async Task<Chapter?> GetByIdAsync(string id) => await _repository.GetByIdAsync(id);
        public async Task<Chapter?> GetByOrderAsync(int storyId, int order) => await _repository.GetByOrderAsync(storyId, order);

        public async Task<string> LoadContentAsync(Chapter chapter) =>
            await _storageService.LoadContentAsync(chapter.Id, chapter.IsStoredInFile, chapter.FilePath);

        public async Task<bool> CreateAsync(Chapter chapter, string content)
        {
            //chapter.WordCount = content.Length;
            chapter.WordCount = TextUtils.CountWords(content);
            chapter.IsStoredInFile = content.Length >= 5000;
            chapter.FilePath = await _storageService.SaveContentAsync(chapter.Id, content);
            chapter.CreatedAt = DateTime.UtcNow;
            chapter.UpdatedAt = DateTime.UtcNow;

            await _repository.AddAsync(chapter);
            _taskQueue.QueueBackgroundTask(async token =>
            {
                try
                {
                    await _storyService.UpdateStatsAsync(chapter.StoryId);
                    await _storyService.UpdateChapterRatePerWeekAsync(chapter.StoryId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Không thể cập nhật thông tin stats (Số chương, Số chữ), tỉ lệ ra chương/tuần của truyện [{chapter.StoryId}].");
                }
            });
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Chapter chapter, string content)
        {
            chapter.UpdatedAt = DateTime.UtcNow;
            chapter.WordCount = content.Length;
            chapter.IsStoredInFile = content.Length >= 5000;
            chapter.FilePath = await _storageService.SaveContentAsync(chapter.Id, content);

            await _repository.UpdateAsync(chapter);
            _taskQueue.QueueBackgroundTask(async token =>
            {
                try
                {
                    await _storyService.UpdateStatsAsync(chapter.StoryId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Không thể cập nhật thông tin stats (Số chương, Số chữ) của truyện [{chapter.StoryId}].");
                }
            });

            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var chapter = await _repository.GetByIdAsync(id);
            if (chapter == null) return false;

            await _storageService.DeleteContentAsync(chapter.Id, chapter.IsStoredInFile, chapter.FilePath);
            await _repository.DeleteAsync(id);
            _taskQueue.QueueBackgroundTask(async token =>
            {
                try
                {
                    await _storyService.UpdateStatsAsync(chapter.StoryId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Không thể cập nhật thông tin stats (Số chương, Số chữ) của truyện [{chapter.StoryId}].");
                }
            });
            return await _repository.SaveChangesAsync();
        }

        public async Task IncreaseReadCountAsync(string chapterId)
        {
            var chapter = await _repository.GetByIdAsync(chapterId);
            if (chapter == null) return;

            chapter.ReadCount++;

            await _repository.UpdateAsync(chapter);
            await _repository.SaveChangesAsync();

            await _storyService.IncreaseReadCountAsync(chapter.StoryId); // 👉 gọi logic bên StoryService
        }

    }

}
