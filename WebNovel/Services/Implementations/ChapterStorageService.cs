using Microsoft.EntityFrameworkCore;
using WebNovel.Data;
using WebNovel.Models;
using WebNovel.Services.Interfaces;

namespace WebNovel.Services.Implementations
{
    public class ChapterStorageService : IChapterStorageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly string _basePath;

        public ChapterStorageService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            _basePath = Path.Combine(env.ContentRootPath, "ChapterFiles");
            Directory.CreateDirectory(_basePath);
        }

        public async Task<string> SaveContentAsync(string chapterId, string content)
        {
            if (content.Length < 5000)
            {
                var existing = await _context.ChapterContents.FindAsync(chapterId);
                if (existing == null)
                    _context.ChapterContents.Add(new ChapterContent { ChapterId = chapterId, Content = content });
                else
                    existing.Content = content;

                await _context.SaveChangesAsync();
                return "db";
            }
            else
            {
                string filename = $"chapter_{chapterId}.txt";
                string path = Path.Combine(_basePath, filename);
                await File.WriteAllTextAsync(path, content);
                return path;
            }
        }

        public async Task<string> LoadContentAsync(string chapterId, bool isStoredInFile, string? filePath)
        {
            if (!isStoredInFile)
            {
                var content = await _context.ChapterContents.FindAsync(chapterId);
                return content?.Content ?? string.Empty;
            }
            else if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                return await File.ReadAllTextAsync(filePath);
            }
            return string.Empty;
        }

        public async Task DeleteContentAsync(string chapterId, bool isStoredInFile, string? filePath)
        {
            if (!isStoredInFile)
            {
                var content = await _context.ChapterContents.FindAsync(chapterId);
                if (content != null)
                {
                    _context.ChapterContents.Remove(content);
                    await _context.SaveChangesAsync();
                }
            }
            else if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
