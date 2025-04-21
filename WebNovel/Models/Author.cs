namespace WebNovel.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string Name { get; set; } // "Ngã Cật Tây Hồng Thị"
        public string Slug { get; set; } // "nga-cat-tay-hong-thi"

        public string Description { get; set; } // Tiểu sử ngắn
        public string AvatarUrl { get; set; }   // Ảnh đại diện
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
