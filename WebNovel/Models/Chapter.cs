using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebNovel.Services.Interfaces;

namespace WebNovel.Models
{
    public class Chapter : ISlugEntity
    {
        [StringLength(450)]
        public string Id { get; set; }
        public int StoryId { get; set; }

        public long ReadCount { get; set; } = 0;
        [StringLength(200)]
        public string? Title { get; set; }
        [StringLength(200)]
        public string Slug { get; set; } = "";
        public int Order { get; set; }

        public long WordCount { get; set; } = 0;
        public bool IsStoredInFile { get; set; }
        [StringLength(500)]
        public string? FilePath { get; set; }
        public DateTime? PostedAt { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsPublic { get; set; } = true;

        [StringLength(450)]
        public string CreatedByUserId { get; set; }

        public int ContributorId { get; set; }

        [NotMapped]
        public string? Content { get; set; }
        [NotMapped]
        public string? StoryName { get; set; }

    }
}
