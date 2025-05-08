using Azure.Core;
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
        private readonly ILookupRepository _lookupRepo;

        public StoryRepository(ApplicationDbContext context, ILookupRepository lookupRepo)
        {
            _context = context;
            _lookupRepo = lookupRepo;
        }

        public async Task<List<Story>> GetAllAsync() =>
            await _context.Stories.AsNoTracking().ToListAsync();

        public async Task<Story?> GetByIdAsync(int id) =>
            await _context.Stories.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

        public async Task<Story?> GetBySlugAsync(string slug) =>
            await _context.Stories.AsNoTracking().FirstOrDefaultAsync(s => s.Slug == slug);

        public async Task<StoryDto?> GetStoryDtoBySlugAsync(string slug)
        {
            var allGenres = await _lookupRepo.GetGenresAsync();
            var allTags = await _lookupRepo.GetTagsAsync();

            var query = from sto in _context.Stories.AsNoTracking()
                        join aut in _context.Authors on sto.AuthorId equals aut.Id
                        join con in _context.Contributors on sto.ContributorId equals con.Id
                        where sto.Slug == slug
                        select new StoryDto
                        {
                            Id = sto.Id,
                            Title = sto.Name,
                            AuthorName = aut.Name,
                            AuthorSlug = aut.Slug,
                            Slug = sto.Slug,
                            CoverUrl = sto.CoverUrl,
                            ContributorName = con.Name,
                            ContributorSlug = con.Slug,
                            GenreNames = sto.GenreIds,
                            TagNames = sto.Tags,
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
            var result = await query.FirstOrDefaultAsync();
            var lstGenre = result.GenreNames.Split(',')
                            .Select(id => int.TryParse(id, out var parsedId) ? parsedId : 0)
                            .Where(id => id != 0 && allGenres.ContainsKey(id))
                            .Select(id => allGenres[id])
                            .ToList();
            var lstTag = result.TagNames.Split(',')
                            .Select(id => int.TryParse(id, out var parsedId) ? parsedId : 0)
                            .Where(id => id != 0 && allTags.ContainsKey(id))
                            .Select(id => allTags[id])
                            .ToList();

            result.ListGenreSlug = lstGenre.Select(s => s.Slug).ToList();
            result.ListGenreName = lstGenre.Select(s => s.Name).ToList();
            result.GenreNames = string.Join(", ", result.ListGenreName);

            result.ListTagSlug = lstTag.Select(s => s.Slug).ToList();
            result.ListTagName = lstTag.Select(s => s.Name).ToList();
            result.TagNames = string.Join(", ", result.ListTagName);

            return result;
        }


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

        public async Task<List<StoryDto>> GetRandomStoriesAsync(int count)
        {
            // 1) Lấy trước list Id ngẫu nhiên
            var randomIds = await _context.Stories
                .AsNoTracking()
                .OrderBy(s => Guid.NewGuid())   // SQL Server sẽ translate thành NEWID()
                .Select(s => s.Id)
                .Take(count)
                .ToListAsync();

            // 2) Load đầy đủ dữ liệu cho những Id đó
            var allGenres = await _lookupRepo.GetAllGenresAsync();
            var allTags = await _lookupRepo.GetAllTagsAsync();

            var query = from sto in _context.Stories
                        join aut in _context.Authors on sto.AuthorId equals aut.Id
                        join con in _context.Contributors on sto.ContributorId equals con.Id
                        where randomIds.Contains(sto.Id)
                        select new StoryDto
                        {
                            Id = sto.Id,
                            Title = sto.Name,
                            AuthorName = aut.Name,
                            Slug = sto.Slug,
                            CoverUrl = sto.CoverUrl,
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
                            CreatedAt = sto.CreatedAt
                        };

            var stories = await query
                .AsNoTracking()
                .ToListAsync();

            return stories;
        }

        public async Task<List<StoryDto>> GetTopVotedStoriesAsync(int count)
        {
            var allGenres = await _lookupRepo.GetAllGenresAsync();
            var allTags = await _lookupRepo.GetAllTagsAsync();

            var query = from sto in _context.Stories.AsNoTracking()
                        join aut in _context.Authors on sto.AuthorId equals aut.Id
                        join con in _context.Contributors on sto.ContributorId equals con.Id
                        orderby sto.TotalVotes descending
                        select new StoryDto
                        {
                            Id = sto.Id,
                            Title = sto.Name,
                            AuthorName = aut.Name,
                            Slug = sto.Slug,
                            CoverUrl = sto.CoverUrl,
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

            return await query
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<StoryDto>> GetTopReadStoriesAsync(int count, string period)
        {
            // Bước 1: xác định fromDate
            DateTime fromDate = period.ToLower() switch
            {
                "week" => DateTime.UtcNow.AddDays(-7),
                "month" => DateTime.UtcNow.AddMonths(-1),
                "year" => DateTime.UtcNow.AddYears(-1),
                _ => DateTime.UtcNow.AddMonths(-1)
            };

            // Bước 2: group reads theo chapter, rồi map lên story và lấy top IDs
            var chapterReads = await _context.ChapterReadByDates
                .Where(x => x.ReadDate >= fromDate)
                .GroupBy(x => x.ChapterId)
                .Select(g => new { ChapterId = g.Key, TotalReads = g.Sum(x => x.ReadCount) })
                .ToListAsync();

            var topStoryIds = await _context.Chapters
                .Where(c => chapterReads.Select(r => r.ChapterId).Contains(c.Id))
                .GroupJoin(chapterReads,
                    c => c.Id,
                    r => r.ChapterId,
                    (c, rg) => new { c.StoryId, Reads = rg.Sum(x => x.TotalReads) })
                .GroupBy(x => x.StoryId)
                .Select(g => new { StoryId = g.Key, TotalReads = g.Sum(x => x.Reads) })
                .OrderByDescending(x => x.TotalReads)
                .Take(count)
                .Select(x => x.StoryId)
                .ToListAsync();

            // Bước 3: project ra StoryDto
            var allGenres = await _lookupRepo.GetAllGenresAsync();
            var allTags = await _lookupRepo.GetAllTagsAsync();

            var dtoQuery = from sto in _context.Stories.AsNoTracking()
                           join aut in _context.Authors on sto.AuthorId equals aut.Id
                           join con in _context.Contributors on sto.ContributorId equals con.Id
                           where topStoryIds.Contains(sto.Id)
                           select new StoryDto
                           {
                               Id = sto.Id,
                               Title = sto.Name,
                               AuthorName = aut.Name,
                               Slug = sto.Slug,
                               CoverUrl = sto.CoverUrl,
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

            var list = await dtoQuery.ToListAsync();

            // Giữ đúng thứ tự topStoryIds
            var orderMap = topStoryIds
                .Select((id, idx) => (id, idx))
                .ToDictionary(x => x.id, x => x.idx);
            list.Sort((a, b) => orderMap[a.Id].CompareTo(orderMap[b.Id]));

            return list;
        }

        public async Task<List<StoryDto>> GetNewStoriesAsync(int count)
        {
            var allGenres = await _lookupRepo.GetAllGenresAsync();
            var allTags = await _lookupRepo.GetAllTagsAsync();

            return await (
                from sto in _context.Stories.AsNoTracking()
                join aut in _context.Authors on sto.AuthorId equals aut.Id
                join con in _context.Contributors on sto.ContributorId equals con.Id
                orderby sto.CreatedAt descending
                select new StoryDto
                {
                    Id = sto.Id,
                    Title = sto.Name,
                    AuthorName = aut.Name,
                    Slug = sto.Slug,
                    CoverUrl = sto.CoverUrl,
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
                }
            )
            .Take(count)
            .ToListAsync();
        }



        public async Task<List<StoryDto>> GetNewChapterStoriesAsync(int count)
        {
            var allGenres = await _lookupRepo.GetAllGenresAsync();
            var allTags = await _lookupRepo.GetAllTagsAsync();

            return await (
                from sto in _context.Stories.AsNoTracking()
                join aut in _context.Authors on sto.AuthorId equals aut.Id
                join con in _context.Contributors on sto.ContributorId equals con.Id
                orderby sto.LastChapterUpdatedAt descending
                select new StoryDto
                {
                    Id = sto.Id,
                    Title = sto.Name,
                    AuthorName = aut.Name,
                    Slug = sto.Slug,
                    CoverUrl = sto.CoverUrl,
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
                }
            )
            .Take(count)
            .ToListAsync();
        }

        public async Task<List<StoryDto>> GetStoriesByStatusAsync(string status)
        {
            var allGenres = await _lookupRepo.GetAllGenresAsync();
            var allTags = await _lookupRepo.GetAllTagsAsync();

            return await (
                from sto in _context.Stories.AsNoTracking()
                join aut in _context.Authors on sto.AuthorId equals aut.Id
                join con in _context.Contributors on sto.ContributorId equals con.Id
                where sto.Status == status
                orderby sto.CreatedAt descending
                select new StoryDto
                {
                    Id = sto.Id,
                    Title = sto.Name,
                    AuthorName = aut.Name,
                    Slug = sto.Slug,
                    CoverUrl = sto.CoverUrl,
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
                }
            )
            .ToListAsync();
        }

        public async Task<DataSourceResult> GetDataSourceAsync(DataSourceRequest request)
        {
            // Lấy toàn bộ genre và tag trước để tránh query lặp
            //var allGenres = await _context.Genres.ToDictionaryAsync(g => g.Id, g => g.Name);
            //var allTags = await _context.Tags.ToDictionaryAsync(t => t.Id, t => t.Name);
            var allGenres = await _lookupRepo.GetAllGenresAsync();
            var allTags = await _lookupRepo.GetAllTagsAsync();

            var query = from sto in _context.Stories
                        join aut in _context.Authors on sto.AuthorId equals aut.Id
                        join con in _context.Contributors on sto.ContributorId equals con.Id
                        select new StoryDto()
                        {
                            Id = sto.Id,
                            Title = sto.Name,
                            AuthorName = aut.Name,
                            Slug = sto.Slug,
                            CoverUrl = sto.CoverUrl,
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
