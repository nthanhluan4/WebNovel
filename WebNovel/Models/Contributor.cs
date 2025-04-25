using System.ComponentModel.DataAnnotations;
using WebNovel.Services.Interfaces;

namespace WebNovel.Models
{
    public class Contributor : ISlugEntity
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }             // Tên nhóm hoặc cá nhân
        [StringLength(200)]
        public string Slug { get; set; }             // URL-friendly ID
        [StringLength(100)]
        public string? Type { get; set; }             // "Dịch giả", "Convert", "Biên tập", v.v.
        [StringLength(2000)]
        public string? Description { get; set; }      // Giới thiệu
        [StringLength(2000)]
        public string? AvatarUrl { get; set; }        // Ảnh đại diện hoặc logo

        [StringLength(450)]
        public string? CreatedByUserId { get; set; }  // Người đăng ký (ApplicationUser.Id)
        public bool IsApproved { get; set; }         // Đã được admin duyệt hay chưa

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
