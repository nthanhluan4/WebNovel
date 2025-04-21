namespace WebNovel.Models.Dtos
{
    public class ChapterCreateModel
    {
        public int StoryId { get; set; }       // Id truyện mẹ
        public string Title { get; set; }      // Tiêu đề chương
        public int Order { get; set; }         // Thứ tự chương
        public string Content { get; set; }    // Nội dung chương
    }
}
