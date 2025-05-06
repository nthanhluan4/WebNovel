using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebNovel.Services.Interfaces;

namespace WebNovel.Models
{
    public class News : ISlugEntity
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string Title { get; set; }
        [StringLength(500)]
        public string Slug { get; set; }
        [StringLength(2000)]
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        [StringLength(500)]
        public string CoverUrl { get; set; }
        public int CategoryId { get; set; }
        [NotMapped]
        public string? CategoryName { get; set; }
        [StringLength(450)]
        public string AuthorId { get; set; }
        [NotMapped]
        public string? AuthorName { get; set; }

        public DateTime? PublishedAt { get; set; }
        public bool IsPublished { get; set; } = false;
        public int ViewCount { get; set; } = 0;


        public bool IsPinned { get; set; } = false;
        public DateTime? PinnedAt { get; set; }
        public int? PinnedPosition { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

}
