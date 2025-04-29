using Microsoft.EntityFrameworkCore;
using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Repositories.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    public class ChapterReadByDateRepository : IChapterReadByDateRepository
    {
        private readonly ApplicationDbContext _context;

        public ChapterReadByDateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChapterReadByDate?> GetByChapterAndDateAsync(string chapterId, DateTime date)
        {
            return await _context.ChapterReadByDates
                .FirstOrDefaultAsync(x => x.ChapterId == chapterId && x.ReadDate.Date == date.Date);
        }

        public async Task AddAsync(ChapterReadByDate entity)
        {
            _context.ChapterReadByDates.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChapterReadByDate entity)
        {
            _context.ChapterReadByDates.Update(entity);
            await _context.SaveChangesAsync();
        }
    }

}
