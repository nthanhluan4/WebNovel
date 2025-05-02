using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
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
        private readonly IChapterReadingService _chapterReadingService;
        private readonly IStoryService _storyService;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<ChapterService> _logger;

        public ChapterService(IChapterRepository repository,
            IChapterStorageService storageService,
            IStoryService storyService,
            IBackgroundTaskQueue taskQueue,
            ILogger<ChapterService> logger,
            IChapterReadingService chapterReadingService)
        {
            _repository = repository;
            _storageService = storageService;
            _storyService = storyService;
            _taskQueue = taskQueue;
            _logger = logger;
            _chapterReadingService = chapterReadingService;
        }

        public async Task<List<Chapter>> GetByStoryIdAsync(int storyId) => await _repository.GetByStoryIdAsync(storyId);
        public async Task<Chapter?> GetByIdAsync(string id) => await _repository.GetByIdAsync(id);
        public async Task<Chapter?> GetByOrderAsync(int storyId, int order) => await _repository.GetByOrderAsync(storyId, order);

        public async Task<string> LoadContentAsync(Chapter chapter) =>
            await _storageService.LoadContentAsync(chapter.Id, chapter.IsStoredInFile, chapter.FilePath);

        public async Task<bool> CreateAsync(Chapter chapter, string content)
        {
            chapter.WordCount = TextUtils.CountWords(content);

            chapter.IsStoredInFile = false;
            chapter.FilePath = null;
            chapter.CreatedAt = DateTime.UtcNow;
            chapter.UpdatedAt = DateTime.UtcNow;

            await _repository.AddAsync(chapter);
            var result = await _repository.SaveChangesAsync();

            await _storyService.UpdateStoryByChapterAction(chapter.StoryId, "Create");
            return result;
        }

        public async Task<bool> UpdateAsync(Chapter chapter, string content)
        {
            var existing = await _repository.GetByIdAsync(chapter.Id);
            if (existing == null) return false;

            // Map fields cần cập nhật
            existing.Title = chapter.Title;
            existing.Order = chapter.Order;
            existing.Slug = $"chuong-{chapter.Order}";
            existing.UpdatedAt = DateTime.UtcNow;
            existing.IsPublic = chapter.IsPublic;
            //existing.IsStoredInFile = chapter.IsStoredInFile;
            //existing.FilePath = chapter.FilePath;
            existing.PostedAt = chapter.PostedAt;
            existing.Content = content; //Gắn vào để repo lưu
            existing.WordCount = TextUtils.CountWords(content);
            existing.ContributorId = chapter.ContributorId;

            await _repository.UpdateAsync(chapter);
            var result = await _repository.SaveChangesAsync();

            await _storyService.UpdateStoryByChapterAction(chapter.StoryId, "Update");
            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var chapter = await _repository.GetByIdAsync(id);
            if (chapter == null) return false;

            await _storageService.DeleteContentAsync(chapter.Id, chapter.IsStoredInFile, chapter.FilePath);
            await _repository.DeleteAsync(id);
            await _storyService.UpdateStoryByChapterAction(chapter.StoryId, "Delete");
            return await _repository.SaveChangesAsync();
        }

        public async Task IncreaseReadCountAsync(string chapterId, string? userId)
        {
            var chapter = await _repository.GetByIdAsync(chapterId);
            if (chapter == null) return;

            chapter.ReadCount++;
            await _repository.UpdateAsync(chapter);

            await _repository.SaveChangesAsync();
            await _storyService.IncreaseReadCountAsync(chapter.StoryId);
            _taskQueue.QueueBackgroundTask(async token =>
            {
                try
                {
                    await _chapterReadingService.RecordChapterReadAsync(userId, chapter.StoryId, chapterId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Không thể cập nhật thông tin stats (Số chương, Số chữ), tỉ lệ ra chương/tuần của truyện [{chapter.StoryId}].");
                }
            });
        }

        public async Task<List<Chapter>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<DataSourceResult> GetAllDataSourceAsync(DataSourceRequest request)
        {
            var result = await _repository.GetAllAsync();
            return await result.ToDataSourceResultAsync(request);
        }

        public async Task<List<Chapter>> GetDropdownDataAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<DataSourceResult> GetDataSourceAsync(DataSourceRequest request)
        {
            return await _repository.GetDataSourceAsync(request);
        }
        public async Task<DataSourceResult> GetDataSourceByStoryAsync(int storyId, DataSourceRequest request)
        {
            return await _repository.GetDataSourceByStoryAsync(storyId, request);
        }
    }

}
