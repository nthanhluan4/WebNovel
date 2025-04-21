using Microsoft.AspNetCore.Identity;

namespace WebNovel.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Bạn có thể thêm các thuộc tính custom như:
        public string? FullName { get; set; }
    }
}
