using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Models.Dtos;
using WebNovel.Repositories.Interfaces;
using WebNovel.Utils;

namespace WebNovel.Repositories.Implementations
{
    public class StoryRepository : IStoryRepository
    {
        private readonly ApplicationDbContext _context;

        public StoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Story>> GetAllAsync() =>
            await _context.Stories.AsNoTracking().ToListAsync();

        public async Task<Story?> GetByIdAsync(int id) =>
            await _context.Stories.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

        public async Task<Story?> GetBySlugAsync(string slug) =>
            await _context.Stories.AsNoTracking().FirstOrDefaultAsync(s => s.Slug == slug);

        public async Task<List<Story>> GetByGenreAsync(int genreId)
        {
            var idStr = "," + genreId.ToString() + ",";
            return await _context.Stories
                .Where(s => ("," + s.GenreIds + ",").Contains(idStr))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Story>> GetByAuthorIdAsync(int authorId) =>
            await _context.Stories
                .Where(s => s.AuthorId == authorId)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Story>> GetByContributorIdAsync(int contributorId) =>
            await _context.Stories
                .Where(s => s.ContributorId == contributorId)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Story>> SearchAsync(string keyword) =>
            await _context.Stories
                .Where(s => s.Name.Contains(keyword) || s.AuthorName.Contains(keyword))
                .AsNoTracking()
                .ToListAsync();

        public async Task AddAsync(Story story) =>
            await _context.Stories.AddAsync(story);

        public Task UpdateAsync(Story story)
        {
            _context.Stories.Update(story);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var story = await _context.Stories.FindAsync(id);
            if (story != null)
            {
                _context.Stories.Remove(story);
            }
        }


        public async Task<int> CountChaptersAsync(int storyId)
        {
            return await _context.Chapters.Where(c => c.StoryId == storyId).CountAsync();
        }

        public async Task<long> SumWordsAsync(int storyId)
        {
            return await _context.Chapters
                .Where(c => c.StoryId == storyId)
                .SumAsync(c => (long?)c.WordCount) ?? 0;
        }


        public async Task<bool> SaveChangesAsync() =>
            (await _context.SaveChangesAsync()) > 0;

        public Task<DataSourceResult> GetGridAsync(DataSourceRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Story>> GetRandomStoriesAsync(int count)
        {
            return await _context.Stories
                .OrderBy(x => Guid.NewGuid())
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Story>> GetTopVotedStoriesAsync(int count)
        {
            return await _context.Stories
                .OrderByDescending(x => x.TotalVotes)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Story>> GetTopReadStoriesAsync(int count, string period)
        {
            DateTime fromDate = period.ToLower() switch
            {
                "week" => DateTime.UtcNow.AddDays(-7),
                "month" => DateTime.UtcNow.AddMonths(-1),
                "year" => DateTime.UtcNow.AddYears(-1),
                _ => DateTime.UtcNow.AddMonths(-1)
            };

            // Step 1: Lấy tổng lượt đọc mỗi chương từ ngày fromDate
            var chapterReadQuery = await _context.ChapterReadByDates
                .Where(x => x.ReadDate >= fromDate)
                .GroupBy(x => x.ChapterId)
                .Select(g => new
                {
                    ChapterId = g.Key,
                    TotalReads = g.Sum(x => x.ReadCount)
                })
                .ToListAsync();

            // Step 2: Join ChapterId -> Chapter -> StoryId
            var topStoryIds = await _context.Chapters
                .Where(c => chapterReadQuery.Select(x => x.ChapterId).Contains(c.Id))
                .GroupJoin(chapterReadQuery,
                    chapter => chapter.Id,
                    read => read.ChapterId,
                    (chapter, readGroup) => new
                    {
                        chapter.StoryId,
                        TotalReads = readGroup.Sum(rg => rg.TotalReads)
                    })
                .GroupBy(x => x.StoryId)
                .Select(g => new
                {
                    StoryId = g.Key,
                    TotalReads = g.Sum(x => x.TotalReads)
                })
                .OrderByDescending(x => x.TotalReads)
                .Take(count)
                .Select(x => x.StoryId)
                .ToListAsync();

            // Step 3: Lấy danh sách Story tương ứng
            var stories = await _context.Stories
                .Where(s => topStoryIds.Contains(s.Id))
                .ToListAsync();

            // Optional: Sort lại đúng thứ tự topStoryIds
            stories = stories.OrderBy(s => topStoryIds.IndexOf(s.Id)).ToList();

            return stories;
        }


        public async Task<List<Story>> GetNewStoriesAsync(int count)
        {
            return await _context.Stories
                .OrderByDescending(x => x.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Story>> GetNewChapterStoriesAsync(int count)
        {
            return await _context.Stories
                .OrderByDescending(x => x.LastChapterUpdatedAt) // Giả định có trường LastChapterUpdatedAt
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Story>> GetStoriesByStatusAsync(string status)
        {
            return await _context.Stories
                .Where(x => x.Status == status)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<DataSourceResult> GetDataSourceAsync(DataSourceRequest request)
        {
            // Lấy toàn bộ genre và tag trước để tránh query lặp
            var allGenres = await _context.Genres.ToDictionaryAsync(g => g.Id, g => g.Name);
            var allTags = await _context.Tags.ToDictionaryAsync(t => t.Id, t => t.Name);


            var query = from sto in _context.Stories
                        join aut in _context.Authors on sto.AuthorId equals aut.Id
                        join con in _context.Contributors on sto.ContributorId equals con.Id
                        select new StoryDto()
                        {
                            Id = sto.Id,
                            Title = sto.Name,
                            AuthorName = aut.Name,
                            ContributorName = con.Name,
                            GenreNames = GetDataHelper.ConvertIdsToNames(sto.GenreIds, allGenres),
                            TagNames = GetDataHelper.ConvertIdsToNames(sto.Tags, allTags),
                            Description = sto.Description,
                            TotalChapters = sto.TotalChapters,
                            TotalWords = sto.TotalWords,
                            TotalVotes = sto.TotalVotes,
                            ReadCount = sto.ReadCount,
                            ViewCount = sto.ViewCount,
                            FollowCount = sto.FollowCount,
                            Status = GetDataHelper.ConvertStoryStatusToNames(sto.Status),
                            Rating = sto.Rating,
                            CreatedAt = sto.CreatedAt,
                        };

            return await query.ToDataSourceResultAsync(request);
        }
    }
}
