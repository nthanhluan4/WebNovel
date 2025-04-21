using Microsoft.AspNetCore.Identity;

namespace WebNovel.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

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
            var adminUser = new IdentityUser { UserName = "admin@webnovel.com", Email = "admin@gmail.com", EmailConfirmed = true };
            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Tạo user Reader
            var readerUser = new IdentityUser { UserName = "reader@webnovel.com", Email = "reader@webnovel.com", EmailConfirmed = true };
            if (await userManager.FindByEmailAsync(readerUser.Email) == null)
            {
                await userManager.CreateAsync(readerUser, "Reader@123");
                await userManager.AddToRoleAsync(readerUser, "Reader");
            }

            // Tạo user Contributor (là Reader đã được nâng quyền)
            var contributorUser = new IdentityUser { UserName = "contributor@webnovel.com", Email = "contributor@webnovel.com", EmailConfirmed = true };
            if (await userManager.FindByEmailAsync(contributorUser.Email) == null)
            {
                await userManager.CreateAsync(contributorUser, "Contributor@123");
                await userManager.AddToRolesAsync(contributorUser, new[] { "Reader", "Contributor" });
            }
        }
    }
}
