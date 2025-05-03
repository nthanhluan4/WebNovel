namespace WebNovel.Models.Dtos
{
    public class StoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string CoverUrl { get; set; } = null!;
        public string? Description { get; set; }
        public string? AuthorName { get; set; }
        public string? ContributorName { get; set; }
        public string? GenreNames { get; set; }
        public string? TagNames { get; set; }
        public int TotalChapters { get; set; }
        public long TotalWords { get; set; }
        public long TotalVotes { get; set; }
        public long ReadCount { get; set; }
        public long ViewCount { get; set; }
        public int FollowCount { get; set; }
        public double Rating { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
