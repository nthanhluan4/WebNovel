﻿using Kendo.Mvc.UI;
using WebNovel.Models;
using WebNovel.Models.Dtos;

namespace WebNovel.Services.Interfaces
{
    public interface IStoryService
    {
        Task<List<Story>> GetAllAsync();
        Task<Story?> GetByIdAsync(int id);
        Task<Story?> GetBySlugAsync(string slug);
        Task<StoryDto?> GetStoryDtoBySlugAsync(string slug);
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


        Task<List<StoryDto>> GetRandomStoriesAsync(int count);
        Task<List<StoryDto>> GetTopVotedStoriesAsync(int count);
        Task<List<StoryDto>> GetTopReadStoriesAsync(int count, string period);
        Task<List<StoryDto>> GetNewStoriesAsync(int count);
        Task<List<StoryDto>> GetNewChapterStoriesAsync(int count);
        Task<List<StoryDto>> GetStoriesByStatusAsync(string status);
        Task<DataSourceResult> GetDataSourceAsync(DataSourceRequest request);


        Task<List<StoryDto>> GetByGenreSlugAsync(string slug);
        Task<List<StoryDto>> GetByAuthorSlugAsync(string slug);
        Task<List<StoryDto>> GetByContributorSlugAsync(string slug);
    }
}
