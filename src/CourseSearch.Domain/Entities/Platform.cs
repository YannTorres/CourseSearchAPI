namespace CourseSearch.Domain.Entities;
/// <summary>
/// Representa uma plataforma de origem de cursos (ex: YouTube, edX).
/// </summary>
public class Platform
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
