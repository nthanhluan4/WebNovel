using WebNovel.Models;

namespace WebNovel.Repositories.Interfaces
{
    public interface IChapterReadByDateRepository
    {
        Task<ChapterReadByDate?> GetByChapterAndDateAsync(string chapterId, DateTime date);
        Task AddAsync(ChapterReadByDate entity);
        Task UpdateAsync(ChapterReadByDate entity);

    }
}
