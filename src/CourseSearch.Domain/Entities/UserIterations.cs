using CourseSearch.Domain.Enums;

namespace CourseSearch.Domain.Entities;
internal class UserIterations : EntityBase
{

    public UserIteration Iteration { get; set; }
    public int IterationValue { get; set; } = 0;
    public Guid CourseId { get; set; } = Guid.Empty;
    // public Course Course { get; set; } = default!;
    public Guid UserId { get; set; } = Guid.Empty;
    public User User { get; set; } = default!;
}
