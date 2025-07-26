namespace CourseSearch.Domain.Entities;
/// <summary>
/// Tipo de interação do usuário com os cursos, como "assistiu", "curtiu", etc.
/// </summary>
public class InteractionType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
