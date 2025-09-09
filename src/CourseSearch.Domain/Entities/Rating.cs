namespace CourseSearch.Domain.Entities;
public class Rating
{
    public int Id { get; set; }
    public int Count { get; set; }
    public float Average { get; set; }
    public Guid CourseId { get; set; } = Guid.Empty;
    public virtual Course Course { get; set; } = null!;
}
