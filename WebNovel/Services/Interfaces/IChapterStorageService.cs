namespace WebNovel.Services.Interfaces
{
    public interface IChapterStorageService
    {
        Task<string> SaveContentAsync(string chapterId, string content);
        Task<string> LoadContentAsync(string chapterId, bool isStoredInFile, string? filePath);
        Task DeleteContentAsync(string chapterId, bool isStoredInFile, string? filePath);
    }
}
