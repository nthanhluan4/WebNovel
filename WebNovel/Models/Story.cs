using WebNovel.Services.Interfaces;

namespace WebNovel.Models
{
    public class Story : ISlugEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public int AuthorId { get; set; } // Thay vì tên chuỗi
        public string AuthorName { get; set; } // Lưu kèm để hiển thị nhanh
        public string Description { get; set; }

        public int ContributorId { get; set; }        // ID người đóng góp chính
        public string ContributorName { get; set; }   // Tên hiển thị nhanh
        public string ContributorType { get; set; }   // Ví dụ: "Dịch giả", "Convert"
        public double ChapterRatePerWeek { get; set; } // Tốc độ ra chương trung bình mỗi tuần
        public string GenreIds { get; set; } // "1,3,4"
        public string Tags { get; set; } // "dị giới, học đường"

        public string Status { get; set; } // "Ongoing", "Completed"
        public string CoverUrl { get; set; }

        public int TotalChapters { get; set; }
        public long TotalWords { get; set; }
        public long ViewCount { get; set; }
        public int FollowCount { get; set; }
        public double Rating { get; set; }

        public long ReadCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedByUserId { get; set; }
    }
}
