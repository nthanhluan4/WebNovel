using WebNovel.Services.Interfaces;

namespace WebNovel.Models
{
    public class Genre : ISlugEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }         // "Tiên hiệp", "Đô thị"
        public string Slug { get; set; }         // "tien-hiep"
        public string Description { get; set; }  // Mô tả ngắn về thể loại
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
