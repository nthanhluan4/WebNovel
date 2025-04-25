using System.ComponentModel.DataAnnotations;
using WebNovel.Services.Interfaces;

namespace WebNovel.Models
{
    public class Tag : ISlugEntity
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string? Name { get; set; } = null!;      // VD: Tu tiên
        [StringLength(200)]
        public string Slug { get; set; } = null!;      // VD: tu-tien
        [StringLength(2000)]
        public string? Description { get; set; }       // (nếu có mô tả tag)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
