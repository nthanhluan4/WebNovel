using System.ComponentModel.DataAnnotations;

namespace WebNovel.Models
{
    public class UserChapterRead
    {
        public int Id { get; set; }
        [StringLength(450)]
        public string UserId { get; set; } = null!;
        public int StoryId { get; set; }
        [StringLength(450)]
        public string ChapterId { get; set; }
        public DateTime ReadAt { get; set; } = DateTime.UtcNow;
    }

}
