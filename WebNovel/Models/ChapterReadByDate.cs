using System.ComponentModel.DataAnnotations;

namespace WebNovel.Models
{
    public class ChapterReadByDate
    {
        public int Id { get; set; }
        [StringLength(450)]
        public string ChapterId { get; set; }
        public DateTime ReadDate { get; set; }
        public long ReadCount { get; set; } = 0;
    }

}
