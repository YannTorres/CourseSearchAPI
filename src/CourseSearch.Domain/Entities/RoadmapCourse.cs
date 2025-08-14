namespace CourseSearch.Domain.Entities;
/// <summary>
/// Entidade de Junção para a relação Muitos-para-Muitos entre Roadmaps e Courses.
/// </summary>
public class RoadmapCourse
{
    public Guid RoadmapId { get; set; }
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseDescription { get; set; } = string.Empty;
    public int StepOrder { get; set; }
    public virtual Roadmap Roadmap { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
}
