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
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text.Json;

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
                PermitLimit = 50,                    // Cho phép tối đa 30 request
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
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver(); // PascalCase
}); ;
builder.Services.AddKendo();
builder.Services.AddMemoryCache();
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN"; // Header name để kiểm tra token
});

builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
//builder.Services.AddScoped<QueuedHostedService>();
builder.Services.AddHostedService<QueuedHostedService>();

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

builder.Services.AddScoped<IStoryVoteRepository, StoryVoteRepository>();
builder.Services.AddScoped<IStoryVoteService, StoryVoteService>();

builder.Services.AddScoped<IChapterReadByDateRepository, ChapterReadByDateRepository>();
builder.Services.AddScoped<IUserChapterReadRepository, UserChapterReadRepository>();
builder.Services.AddScoped<IChapterReadingService, ChapterReadingService>();


builder.Services.AddScoped<ISlugRepository<Story>, SlugRepository<Story>>();
builder.Services.AddScoped<ISlugService<Story>, SlugService<Story>>();
builder.Services.AddScoped<ISlugRepository<Tag>, SlugRepository<Tag>>();
builder.Services.AddScoped<ISlugService<Tag>, SlugService<Tag>>();
builder.Services.AddScoped<ISlugRepository<Chapter>, SlugRepository<Chapter>>();
builder.Services.AddScoped<ISlugService<Chapter>, SlugService<Chapter>>();
builder.Services.AddScoped<ISlugRepository<Genre>, SlugRepository<Genre>>();
builder.Services.AddScoped<ISlugService<Genre>, SlugService<Genre>>();
builder.Services.AddScoped<IBaseRepository<Rating>, BaseRepository<Rating>>();
builder.Services.AddScoped<IBaseService<Rating>, BaseService<Rating>>();
builder.Services.AddScoped<ISlugRepository<Author>, SlugRepository<Author>>();
builder.Services.AddScoped<ISlugService<Author>, SlugService<Author>>();
builder.Services.AddScoped<ISlugRepository<Contributor>, SlugRepository<Contributor>>();
builder.Services.AddScoped<ISlugService<Contributor>, SlugService<Contributor>>();
builder.Services.AddScoped<IModel>(provider =>
{
    var dbContext = provider.GetRequiredService<ApplicationDbContext>();
    return dbContext.Model;
});
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
                var result = JsonSerializer.Serialize(new { Success = false, Message = dup.Message },
                        new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = null // 👈 giữ nguyên PascalCase
                        });

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var result = JsonSerializer.Serialize(new { Success = false, Message = "Lỗi hệ thống, vui lòng thử lại sau." },
                        new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = null // 👈 giữ nguyên PascalCase
                        });

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }
        }
        else
        {
            //context.Response.Redirect("/Home/Error");
        }
    });
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
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
