namespace CourseSearch.Domain.Entities;
public class UserCourseRating
{
    public Guid UserId { get; set; } 
    public Guid CourseId { get; set; } 
    public int Score { get; set; } 
    public string? Review { get; set; }
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    public virtual User User { get; set; }
    public virtual Course Course { get; set; }
}
