using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Models.Dtos;
using WebNovel.Repositories.Interfaces;
using WebNovel.Utils;

namespace WebNovel.Repositories.Implementations
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly ApplicationDbContext _context;

        public ChapterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Chapter>> GetByStoryIdAsync(int storyId) =>
            await _context.Chapters.Where(c => c.StoryId == storyId).OrderBy(c => c.Order).ToListAsync();

        public async Task<Chapter?> GetByIdAsync(string id) =>
            await _context.Chapters.FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Chapter?> GetByOrderAsync(int storyId, int order) =>
            await _context.Chapters.FirstOrDefaultAsync(c => c.StoryId == storyId && c.Order == order);

        public async Task AddAsync(Chapter chapter)
        {
            var maxOrder = await _context.Chapters
                .Where(c => c.StoryId == chapter.StoryId)
                .Select(c => (int?)c.Order)
                .MaxAsync() ?? 0;
            chapter.Order = maxOrder + 1;
            chapter.Slug = $"chuong-{chapter.Order}";

            await _context.Chapters.AddAsync(chapter);
            await _context.ChapterContents.AddAsync(new ChapterContent()
            {
                Content = chapter.Content,
                ChapterId = chapter.Id,
            });
        }

        public async Task UpdateAsync(Chapter chapter)
        {
            _context.Chapters.Update(chapter);

            var chapterContent = await _context.ChapterContents.FirstOrDefaultAsync(s => s.ChapterId == chapter.Id);
            if(chapterContent != null)
            {
                chapterContent.Content = chapter.Content;
                _context.ChapterContents.Update(chapterContent);
            }
            //return Task.CompletedTask;
        }

        public async Task DeleteAsync(string id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            var orderDeleted = 0;
            var storyId = 0;
            if (chapter != null)
            {
                orderDeleted = chapter.Order;
                storyId = chapter.StoryId;
                _context.Chapters.Remove(chapter);
            }
            var chapterContent = await _context.ChapterContents.FirstOrDefaultAsync(s => s.ChapterId == id);
            if (chapterContent != null) _context.ChapterContents.Remove(chapterContent);
            if (storyId != 0)
            {
                var chaptersToUpdate = await _context.Chapters
                .Where(c => c.StoryId == storyId && c.Order > orderDeleted)
                .ToListAsync();
                foreach (var c in chaptersToUpdate)
                {
                    c.Order--;
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SaveChangesAsync() => (await _context.SaveChangesAsync()) > 0;

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

        public async Task<List<Chapter>> GetAllAsync()
        {
            return await _context.Chapters.AsNoTracking().ToListAsync();
        }
        public async Task<DataSourceResult> GetDataSourceAsync(DataSourceRequest request)
        {
            var query = from cha in _context.Chapters
                        join sto in _context.Stories on cha.StoryId equals sto.Id
                        select new ChapterDto()
                        {
                            Id = cha.Id,
                            Title = cha.Title,
                            StoryName = sto.Name,
                            WordCount = cha.WordCount,
                            ReadCount = cha.ReadCount,
                            Order = cha.Order,
                            CreatedAt = cha.CreatedAt,
                            UpdatedAt = cha.UpdatedAt,
                            IsPublic = cha.IsPublic,
                        }; 

            return await query.ToDataSourceResultAsync(request);
        }
    }
}
