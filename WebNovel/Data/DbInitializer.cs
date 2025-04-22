using Microsoft.AspNetCore.Identity;
using WebNovel.Models;

namespace WebNovel.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = new[] { "Admin", "Reader", "Contributor" };

            // Tạo các role nếu chưa có
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Tạo user Admin
            var adminUser = new ApplicationUser { UserName = "admintruyencity@gmail.com", Email = "admintruyencity@gmail.com", EmailConfirmed = true };
            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                await userManager.CreateAsync(adminUser, "Admin@@12345");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Tạo user Reader
            var readerUser = new ApplicationUser { UserName = "user001", Email = "user001@gmail.com", EmailConfirmed = true };
            if (await userManager.FindByEmailAsync(readerUser.Email) == null)
            {
                await userManager.CreateAsync(readerUser, "User001@@123");
                await userManager.AddToRoleAsync(readerUser, "Reader");
            }

            // Tạo user Contributor (là Reader đã được nâng quyền)
            var contributorUser = new ApplicationUser { UserName = "truyencity", Email = "truyencity@gmail.com", EmailConfirmed = true };
            if (await userManager.FindByEmailAsync(contributorUser.Email) == null)
            {
                await userManager.CreateAsync(contributorUser, "TruyenCity@@123");
                await userManager.AddToRolesAsync(contributorUser, new[] { "Reader", "Contributor" });
            }


            // Tạo user Contributor (là Reader đã được nâng quyền)
            var contributorUser1 = new ApplicationUser { UserName = "truytim", Email = "truytimcovat551@gmail.com", EmailConfirmed = true };
            if (await userManager.FindByEmailAsync(contributorUser1.Email) == null)
            {
                await userManager.CreateAsync(contributorUser1, "TruyTimCV123@123");
                await userManager.AddToRolesAsync(contributorUser1, new[] { "Reader", "Contributor" });
            }
        }
    }
}
