using System.ComponentModel.DataAnnotations;

namespace WebNovel.Models
{
    public class ChapterContent
    {
        [StringLength(450)]
        public string ChapterId { get; set; }
        public string? Content { get; set; }
    }
}
