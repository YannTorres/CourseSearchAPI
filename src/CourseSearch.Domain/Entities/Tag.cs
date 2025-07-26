namespace CourseSearch.Domain.Entities;
/// <summary>
/// Tags relacionadas a cursos, como "Python", "JavaScript", etc.
/// </summary>
public class Tag
{
   public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
