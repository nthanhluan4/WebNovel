using System.ComponentModel.DataAnnotations;
using WebNovel.Services.Interfaces;

namespace WebNovel.Models
{
    public class Story : ISlugEntity
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Slug { get; set; }
        public int AuthorId { get; set; } // Thay vì tên chuỗi
        [StringLength(200)]
        public string? AuthorName { get; set; } // Lưu kèm để hiển thị nhanh
        [StringLength(2000)]
        public string? Description { get; set; }

        public int ContributorId { get; set; }        // ID người đóng góp chính
        [StringLength(200)]
        public string? ContributorName { get; set; }   // Tên hiển thị nhanh
        [StringLength(200)]
        public string? ContributorType { get; set; }   // Ví dụ: "Dịch giả", "Convert"
        public double ChapterRatePerWeek { get; set; } // Tốc độ ra chương trung bình mỗi tuần
        public string? GenreIds { get; set; } // "1,3,4"
        public string? Tags { get; set; } // "dị giới, học đường"

        [StringLength(200)]
        public string? Status { get; set; } // "Ongoing", "Completed"
        [StringLength(400)]
        public string? CoverUrl { get; set; }

        public long ViewCount { get; set; } = 0;
        public int FollowCount { get; set; } = 0;
        public long RatingCount { get; set; } = 0;
        public long CommentCount { get; set; } = 0;
        public double Rating { get; set; } = 0;


        public long ReadCount { get; set; } = 0;
        public long TotalVotes { get; set; } = 0;
        public long TotalWords { get; set; } = 0;
        public int TotalChapters { get; set; } = 0;
        public DateTime? LastChapterUpdatedAt { get; set; }

        public bool IsPublic { get; set; } = true;
        public DateTime? PostedAt { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [StringLength(450)]
        public string CreatedByUserId { get; set; }
    }
}
