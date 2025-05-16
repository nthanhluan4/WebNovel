using Kendo.Mvc.UI;
using WebNovel.Models;
using WebNovel.Models.Dtos;

namespace WebNovel.Services.Interfaces
{
    public interface IChapterService
    {
        Task<List<Chapter>> GetAllAsync();
        Task<DataSourceResult> GetAllDataSourceAsync(DataSourceRequest request);
        Task<List<Chapter>> GetDropdownDataAsync();
        Task<List<Chapter>> GetByStoryIdAsync(int storyId);
        Task<Chapter?> GetByIdAsync(string id);
        Task<Chapter?> GetByOrderAsync(int storyId, int order);
        Task<bool> CreateAsync(Chapter chapter, string content);
        Task<bool> UpdateAsync(Chapter chapter, string content);
        Task<bool> DeleteAsync(string id);
        Task<string> LoadContentAsync(Chapter chapter);

        //Tăng lượt đọc cho chương
        Task IncreaseReadCountAsync(string chapterId, string? userId);
        Task<DataSourceResult> GetDataSourceAsync(DataSourceRequest request);
        Task<DataSourceResult> GetDataSourceByStoryAsync(int storyId, DataSourceRequest request);

        Task<List<ChapterDto>> GetNewChapterByStoryIdAsync(int storyId, int take);
        Task<ChapterDto> GetChapterReadingAsync(string storySlug, string chapterSlug);
    }
}
