using Kendo.Mvc.UI;
using WebNovel.Models;
using WebNovel.Models.Dtos;

namespace WebNovel.Repositories.Interfaces
{
    public interface IStoryRepository
    {
        Task<List<Story>> GetAllAsync();
        Task<Story?> GetByIdAsync(int id);
        Task<Story?> GetBySlugAsync(string slug);
        Task<StoryDto?> GetStoryDtoBySlugAsync(string slug);
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
        Task<List<StoryDto>> GetRandomStoriesAsync(int count);//Ngẫu nhiên
        Task<List<StoryDto>> GetTopVotedStoriesAsync(int count);//Top đề cử
        Task<List<StoryDto>> GetTopReadStoriesAsync(int count, string period);//Top lượt đọc
        Task<List<StoryDto>> GetNewStoriesAsync(int count);//Truyện mới
        Task<List<StoryDto>> GetNewChapterStoriesAsync(int count);//Truyện có chương mới
        Task<List<StoryDto>> GetStoriesByStatusAsync(string status);//Theo trạng thái
        Task<DataSourceResult> GetDataSourceAsync(DataSourceRequest request);//Lưới


        Task<List<StoryDto>> GetByGenreSlugAsync(string slug);
        Task<List<StoryDto>> GetByAuthorSlugAsync(string slug);
        Task<List<StoryDto>> GetByContributorSlugAsync(string slug);
    }
}
