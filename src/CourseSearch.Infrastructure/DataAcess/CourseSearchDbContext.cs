using CourseSearch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseSearch.Infrastructure.DataAcess;
internal class CourseSearchDbContext : DbContext
{
    public CourseSearchDbContext(DbContextOptions<CourseSearchDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Roadmap> Roadmaps { get; set; }
    public DbSet<RoadmapCourse> RoadmapCourses { get; set; }
    public DbSet<InteractionType> InteractionTypes { get; set; }
    public DbSet<UserInteraction> UserInteractions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RoadmapCourse>()
            .HasKey(rc => new { rc.RoadmapId, rc.CourseId });

        modelBuilder.Entity<Course>()
            .HasMany(c => c.Tags) 
            .WithMany(t => t.Courses)
            .UsingEntity<Dictionary<string, object>>(
                "CoursesTags", 
                j => j
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("TagId"), 
                j => j
                    .HasOne<Course>()
                    .WithMany()
                    .HasForeignKey("CourseId"), 
                j =>
                {
                    j.ToTable("CoursesTags");
                    j.HasKey("CourseId", "TagId");
                });
    }
}
