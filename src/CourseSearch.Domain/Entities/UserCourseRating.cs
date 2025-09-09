namespace CourseSearch.Domain.Entities;
public class UserCourseRating
{
    public Guid UserId { get; set; } 
    public Guid CourseId { get; set; } 
    public int Score { get; set; } 
    public virtual User User { get; set; } = new User();
    public virtual Course Course { get; set; } = new Course();
}
