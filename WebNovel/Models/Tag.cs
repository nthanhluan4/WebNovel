using WebNovel.Services.Interfaces;

namespace WebNovel.Models
{
    public class Tag : ISlugEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;      // VD: Tu tiên
        public string Slug { get; set; } = null!;      // VD: tu-tien
        public string? Description { get; set; }       // (nếu có mô tả tag)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
