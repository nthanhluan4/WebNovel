using Kendo.Mvc.UI;
using WebNovel.Models;
using WebNovel.Models.Dtos;

namespace WebNovel.Repositories.Interfaces
{
    public interface IChapterRepository
    {
        Task<List<Chapter>> GetAllAsync();
        Task<List<Chapter>> GetByStoryIdAsync(int storyId);
        Task<Chapter?> GetByIdAsync(string id);
        Task<Chapter?> GetByOrderAsync(int storyId, int order);
        Task AddAsync(Chapter chapter);
        Task UpdateAsync(Chapter chapter);
        Task IncreaseReadCountAsync(string chapterSlug, string storySlug, string? userId);
        Task DeleteAsync(string id);

        Task<int> CountChaptersAsync(int storyId);
        Task<long> SumWordsAsync(int storyId);

        Task<bool> SaveChangesAsync();
        Task<DataSourceResult> GetDataSourceAsync(DataSourceRequest request);
        Task<DataSourceResult> GetDataSourceByStoryAsync(int storyId, DataSourceRequest request);


        Task<List<ChapterDto>> GetNewChapterByStoryIdAsync(int storyId, int take);
        Task<ChapterDto> GetChapterReadingAsync(string storySlug, string chapterSlug);

    }

}
