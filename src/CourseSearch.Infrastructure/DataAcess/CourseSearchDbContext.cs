using CourseSearch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseSearch.Infrastructure.DataAcess;
internal class CourseSearchDbContext : DbContext
{
    public CourseSearchDbContext(DbContextOptions<CourseSearchDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<UserCourseRating> UserCourseRating { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Roadmap> Roadmaps { get; set; }
    public DbSet<RoadmapCourse> RoadmapCourses { get; set; }
    public DbSet<InteractionType> InteractionTypes { get; set; }
    public DbSet<UserInteraction> UserInteractions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RoadmapCourse>()
            .HasKey(rc => new { rc.RoadmapId, rc.CourseId });

        modelBuilder.Entity<Platform>().HasData(
            new Platform { Id = 1, Name = "edX" },
            new Platform { Id = 2, Name = "Microsoft Learn" },
            new Platform { Id = 3, Name = "Alura" }
            );

        modelBuilder.Entity<UserCourseRating>()
            .HasKey(ucr => new { ucr.UserId, ucr.CourseId });
    }
}
