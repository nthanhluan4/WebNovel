using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebNovel.Models;

namespace WebNovel.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryVote> StoryVotes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Contributor> Contributors { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<ChapterContent> ChapterContents { get; set; }
        public DbSet<ChapterReadByDate> ChapterReadByDates { get; set; }
        public DbSet<UserChapterRead> UserChapterReads { get; set; }
        public DbSet<News> News { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Genre>()
                .HasIndex(g => g.Name)
                .IsUnique();

            builder.Entity<Author>()
                .HasIndex(a => a.Slug)
                .IsUnique();

            builder.Entity<Contributor>()
                .HasIndex(c => c.Slug)
                .IsUnique();

            builder.Entity<Tag>()
                .HasIndex(a => a.Slug)
                .IsUnique();

            builder.Entity<Story>()
                .HasIndex(s => s.Slug)
                .IsUnique();

            builder.Entity<Chapter>()
                .HasKey(c => c.Id);

            builder.Entity<ChapterContent>()
                .HasKey(c => c.ChapterId);
        }
    }
}