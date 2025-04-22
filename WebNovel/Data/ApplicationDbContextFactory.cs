using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WebNovel.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // THAY bằng chuỗi thực tế bạn đang dùng (copy từ appsettings.json)
        optionsBuilder.UseSqlServer("Data Source=SQL1003.site4now.net;Initial Catalog=db_ab7e78_webnovel;User Id=db_ab7e78_webnovel_admin;Password=7pKBDwgHMNEd8bA;Integrated Security=False;Trust Server Certificate=True;MultipleActiveResultSets=True;");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
