using Microsoft.EntityFrameworkCore.Migrations;
using WebNovel.Models;
using WebNovel.Repositories.Implementations;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Interfaces;

namespace WebNovel.Services.Implementations
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _repository;
        private readonly IChapterRepository _chapterRepository;

        public StoryService(IStoryRepository repository, IChapterRepository chapterRepository)
        {
            _repository = repository;
            _chapterRepository = chapterRepository;
        }

        public async Task<List<Story>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<Story?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<Story?> GetBySlugAsync(string slug) => await _repository.GetBySlugAsync(slug);

        public async Task<List<Story>> GetByGenreAsync(int genreId) => await _repository.GetByGenreAsync(genreId);

        public async Task<List<Story>> GetByAuthorIdAsync(int authorId) => await _repository.GetByAuthorIdAsync(authorId);

        public async Task<List<Story>> GetByContributorIdAsync(int contributorId) => await _repository.GetByContributorIdAsync(contributorId);

        public async Task<List<Story>> SearchAsync(string keyword) => await _repository.SearchAsync(keyword);

        public async Task<bool> CreateAsync(Story story)
        {
            story.CreatedAt = DateTime.UtcNow;
            story.UpdatedAt = DateTime.UtcNow;
            await _repository.AddAsync(story);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Story story)
        {
            story.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(story);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return await _repository.SaveChangesAsync();
        }

        public async Task UpdateStatsAsync(int storyId)
        {
            var chapterCount = await _repository.CountChaptersAsync(storyId);
            var wordCount = await _repository.SumWordsAsync(storyId);

            var story = await _repository.GetByIdAsync(storyId);
            if (story != null)
            {
                story.TotalChapters = chapterCount;
                story.TotalWords = wordCount;
                await _repository.UpdateAsync(story);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task UpdateChapterRatePerWeekAsync(int storyId)
        {
            var chapters = await _chapterRepository.GetByStoryIdAsync(storyId);
            if (chapters.Count == 0) return;

            var firstDate = chapters.Min(c => c.CreatedAt);
            var now = DateTime.UtcNow;
            var totalWeeks = (now - firstDate).TotalDays / 7.0;
            if (totalWeeks < 1) totalWeeks = 1;

            var rate = Math.Round(chapters.Count / totalWeeks, 2);

            var story = await _repository.GetByIdAsync(storyId);
            if (story != null)
            {
                story.ChapterRatePerWeek = rate;
                story.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(story);
                await _repository.SaveChangesAsync();
            }
        }


        public async Task IncreaseReadCountAsync(int storyId)
        {
            var story = await _repository.GetByIdAsync(storyId);
            if (story == null) return;

            story.ReadCount++;
            await _repository.UpdateAsync(story);
            await _repository.SaveChangesAsync();
        }
    }
}
