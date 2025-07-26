namespace CourseSearch.Domain.Entities;
/// <summary>
/// Registra uma interação específica de um usuário com um curso.
/// </summary>
public class UserInteraction
{
    public long Id { get; set; }
    public int? Rating { get; set; } 
    public DateTime InteractionDate { get; set; }
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    public int InteractionTypeId { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
    public virtual InteractionType InteractionType { get; set; } = null!;
}
