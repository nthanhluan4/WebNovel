using System.ComponentModel.DataAnnotations;
using WebNovel.Services.Interfaces;

namespace WebNovel.Models
{
    public class Author : ISlugEntity
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string? Name { get; set; } // "Ngã Cật Tây Hồng Thị"
        [StringLength(200)]
        public string Slug { get; set; } // "nga-cat-tay-hong-thi"

        [StringLength(2000)]
        public string? Description { get; set; } // Tiểu sử ngắn
        [StringLength(200)]
        public string? AvatarUrl { get; set; }   // Ảnh đại diện
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
