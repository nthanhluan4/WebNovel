namespace WebNovel.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int StoryId { get; set; }        // Truyện được đánh giá
        public int? ChapterId { get; set; }     // Nếu có đánh giá tại 1 chương cụ thể
        public string UserId { get; set; } = null!;  // Người dùng đánh giá

        public int Stars { get; set; }          // Từ 1 đến 5 sao
        public string? Comment { get; set; }    // Ghi chú/đánh giá kèm theo (nếu có)

    }

}
