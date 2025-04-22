using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebNovel.Data;
using WebNovel.Models;
using System.Threading.RateLimiting;
using WebNovel.Services.Interfaces;
using WebNovel.Services;
using WebNovel.Repositories.Implementations;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Implementations;
using Microsoft.AspNetCore.Diagnostics;
using WebNovel.Exceptions;

var builder = WebApplication.CreateBuilder(args);
// KẾT NỐI DATABASE
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// THÊM IDENTITY
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

//Rate Limiter
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 30,                    // Cho phép tối đa 30 request
                Window = TimeSpan.FromSeconds(10),  // Trong mỗi 10 giây
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0                      // Không xếp hàng
            });
    });

    // Tuỳ chọn thêm lỗi 429 response
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddScoped<QueuedHostedService>();

//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddScoped<IStoryService, StoryService>();

builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IChapterStorageService, ChapterStorageService>();
builder.Services.AddScoped<IChapterService, ChapterService>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services.AddScoped<IContributorRepository, ContributorRepository>();
builder.Services.AddScoped<IContributorService, ContributorService>();

builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagService, TagService>();

builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IRatingService, RatingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = feature?.Error;

        var isApiRequest = context.Request.Path.StartsWithSegments("/api") ||
                           context.Request.Headers["Accept"].Any(h => h.Contains("application/json"));

        if (isApiRequest)
        {
            context.Response.ContentType = "application/json";

            if (exception is DuplicateDataException dup)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsJsonAsync(new { message = dup.Message });
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { message = "Lỗi hệ thống, vui lòng thử lại sau." });
            }
        }
        else
        {
            context.Response.Redirect("/Home/Error");
        }
    });
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseRateLimiter(); 

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await WebNovel.Data.DbInitializer.SeedAsync(services);
}

app.Run();
