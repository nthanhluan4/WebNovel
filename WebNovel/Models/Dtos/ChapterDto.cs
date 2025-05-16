namespace WebNovel.Models.Dtos
{
    public class ChapterDto
    {
        public string Id { get; set; }      
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; } = String.Empty;
        public int Order { get; set; }
        public string StoryName { get; set; }
        public string StorySlug { get; set; }
        public long WordCount { get; set; } 
        public long ReadCount { get; set; }
        public string DisplayTimeAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsPublic { get; set; } = true;
    }
}
