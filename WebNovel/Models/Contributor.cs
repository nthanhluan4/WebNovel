namespace WebNovel.Models
{
    public class Contributor
    {
        public int Id { get; set; }

        public string Name { get; set; }             // Tên nhóm hoặc cá nhân
        public string Slug { get; set; }             // URL-friendly ID
        public string Type { get; set; }             // "Dịch giả", "Convert", "Biên tập", v.v.
        public string Description { get; set; }      // Giới thiệu
        public string AvatarUrl { get; set; }        // Ảnh đại diện hoặc logo

        public string CreatedByUserId { get; set; }  // Người đăng ký (ApplicationUser.Id)
        public bool IsApproved { get; set; }         // Đã được admin duyệt hay chưa

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
