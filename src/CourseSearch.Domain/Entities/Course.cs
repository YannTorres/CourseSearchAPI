using CourseSearch.Domain.Enums;

namespace CourseSearch.Domain.Entities;
public class Course : EntityBase
{
    public string ExternalId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CourseUrl { get; set; } = string.Empty;
    public string? Author { get; set; }
    public string? IconUrl { get; set; }
    public int? DurationsInMinutes { get; set; }
    public string? Locale { get; set; }
    public List<CourseLevel>? CourseLevels { get; set; }
    public List<string>? Units { get; set; }
    public float? Popularity { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int PlatformId { get; set; }
    public int? RatingId { get; set; }
    public virtual Platform Platform { get; set; } = null!;
    public virtual Rating? Rating { get; set; }
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public virtual ICollection<RoadmapCourse> Roadmaps { get; set; } = new List<RoadmapCourse>();
    public virtual ICollection<UserInteraction> Interactions { get; set; } = new List<UserInteraction>();
}
