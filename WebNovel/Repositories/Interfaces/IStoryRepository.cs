using Kendo.Mvc.UI;
using WebNovel.Models;

namespace WebNovel.Repositories.Interfaces
{
    public interface IStoryRepository
    {
        Task<List<Story>> GetAllAsync();
        Task<Story?> GetByIdAsync(int id);
        Task<Story?> GetBySlugAsync(string slug);
        Task<List<Story>> GetByGenreAsync(int genreId);
        Task<List<Story>> GetByAuthorIdAsync(int authorId);
        Task<List<Story>> GetByContributorIdAsync(int contributorId);
        Task<List<Story>> SearchAsync(string keyword);

        Task AddAsync(Story story);
        Task UpdateAsync(Story story);
        Task DeleteAsync(int id);

        Task<int> CountChaptersAsync(int storyId);
        Task<long> SumWordsAsync(int storyId);

        Task<bool> SaveChangesAsync();


        //Bổ sung
        Task<List<Story>> GetRandomStoriesAsync(int count);
        Task<List<Story>> GetTopVotedStoriesAsync(int count);
        Task<List<Story>> GetTopReadStoriesAsync(int count, string period);
        Task<List<Story>> GetNewStoriesAsync(int count);
        Task<List<Story>> GetNewChapterStoriesAsync(int count);
        Task<List<Story>> GetStoriesByStatusAsync(string status);
        Task<DataSourceResult> GetDataSourceAsync(DataSourceRequest request);
    }
}
