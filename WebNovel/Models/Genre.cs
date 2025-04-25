using System.ComponentModel.DataAnnotations;
using WebNovel.Services.Interfaces;

namespace WebNovel.Models
{
    public class Genre : ISlugEntity
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string? Name { get; set; }         // "Tiên hiệp", "Đô thị"
        [StringLength(200)]
        public string Slug { get; set; }         // "tien-hiep"
        [StringLength(2000)]
        public string? Description { get; set; }  // Mô tả ngắn về thể loại
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
