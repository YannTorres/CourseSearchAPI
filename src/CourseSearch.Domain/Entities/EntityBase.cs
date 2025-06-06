namespace CourseSearch.Domain.Entities;
public abstract class EntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
