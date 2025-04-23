using WebNovel.Services.Interfaces;

namespace WebNovel.Models
{
    public class Chapter : ISlugEntity
    {
        public string Id { get; set; }
        public int StoryId { get; set; }
        public long ReadCount { get; set; } = 0;
        public string Title { get; set; }
        public string Slug { get; set; } = "";
        public int Order { get; set; }

        public long WordCount { get; set; }
        public bool IsStoredInFile { get; set; }
        public string? FilePath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public string CreatedByUserId { get; set; }
    }
}
