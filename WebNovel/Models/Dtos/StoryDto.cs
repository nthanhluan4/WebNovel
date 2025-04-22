namespace WebNovel.Models.Dtos
{
    public class StoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? AuthorName { get; set; }
        public int TotalChapters { get; set; }
        public long TotalWords { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
