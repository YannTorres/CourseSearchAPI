namespace CourseSearch.Domain.Entities;
/// <summary>
/// Usuário do sistema, que pode criar roadmaps e interagir com cursos.
/// </summary>
public class User : EntityBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public virtual ICollection<Roadmap> CreatedRoadmaps { get; set; } = new List<Roadmap>();
    public virtual ICollection<UserInteraction> Interactions { get; set; } = new List<UserInteraction>();
}
