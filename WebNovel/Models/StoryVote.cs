using System.ComponentModel.DataAnnotations;

namespace WebNovel.Models
{
    public class StoryVote
    {
        public int Id { get; set; }
        [StringLength(450)]
        public string UserId { get; set; } = null!;
        public int StoryId { get; set; }
        public long VoteCount { get; set; } = 1; // Số lượng đề cử
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
