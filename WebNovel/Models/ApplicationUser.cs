using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebNovel.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Bạn có thể thêm các thuộc tính custom như:
        [StringLength(200)]
        public string? FullName { get; set; }
        public long VoteTickets { get; set; } = 0;
        public double Coins { get; set; } = 0;
    }
}
