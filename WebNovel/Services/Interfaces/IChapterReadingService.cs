namespace WebNovel.Services.Interfaces
{
    public interface IChapterReadingService
    {
        Task RecordChapterReadAsync(string? userId, int storyId, string chapterId);
        Task<List<string>> GetUserReadChapterIdsAsync(string userId, int storyId);
    }

}
