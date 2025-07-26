namespace CourseSearch.Domain.Entities;
/// <summary>
/// Curso de aprendizado, com informações como título, descrição e plataforma.
/// </summary>
public class Course : EntityBase
{
    public string ExternalId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CourseUrl { get; set; } = string.Empty;
    public string? Author { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int PlatformId { get; set; }
    public virtual Platform Platform { get; set; } = null!;
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public virtual ICollection<RoadmapCourse> Roadmaps { get; set; } = new List<RoadmapCourse>();
    public virtual ICollection<UserInteraction> Interactions { get; set; } = new List<UserInteraction>();
}
