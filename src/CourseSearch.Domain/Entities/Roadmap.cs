namespace CourseSearch.Domain.Entities;
/// <summary>
/// Representa uma trilha de estudos de um usuário.
/// </summary>
public class Roadmap : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public Guid CreatorUserId { get; set; }
    public virtual User Creator { get; set; } = null!;
    public virtual ICollection<RoadmapCourse> Courses { get; set; } = new List<RoadmapCourse>();
}
