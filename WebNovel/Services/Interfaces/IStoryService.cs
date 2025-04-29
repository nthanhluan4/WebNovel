using Kendo.Mvc.UI;
using WebNovel.Models;

namespace WebNovel.Services.Interfaces
{
    public interface IStoryService
    {
        Task<List<Story>> GetAllAsync();
        Task<Story?> GetByIdAsync(int id);
        Task<Story?> GetBySlugAsync(string slug);
        Task<List<Story>> GetByGenreAsync(int genreId);
        Task<List<Story>> GetByAuthorIdAsync(int authorId);
        Task<List<Story>> GetByContributorIdAsync(int contributorId);
        Task<List<Story>> SearchAsync(string keyword);

        Task<bool> CreateAsync(Story story);
        Task<bool> UpdateAsync(Story story);
        Task<bool> DeleteAsync(int id);

        Task UpdateStatsAsync(int storyId); //cập nhật thông tin số từ, số chương của truyện
        Task IncreaseReadCountAsync(int storyId); // tăng lượt đọc của truyện
        Task UpdateChapterRatePerWeekAsync(int storyId); //  tính tỉ lệ ra chương


        Task UpdateStoryByChapterAction(int storyId, string chapterAction = "Create");


        Task<List<Story>> GetRandomStoriesAsync(int count);
        Task<List<Story>> GetTopVotedStoriesAsync(int count);
        Task<List<Story>> GetTopReadStoriesAsync(int count, string period); // week, month, year
        Task<List<Story>> GetNewStoriesAsync(int count);
        Task<List<Story>> GetNewChapterStoriesAsync(int count);
        Task<List<Story>> GetStoriesByStatusAsync(string status);
        Task<DataSourceResult> GetDataSourceAsync(DataSourceRequest request);
    }
}
