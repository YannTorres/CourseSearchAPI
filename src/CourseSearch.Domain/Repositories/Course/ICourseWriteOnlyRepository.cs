namespace CourseSearch.Domain.Repositories.Course;
public interface ICourseWriteOnlyRepository
{
    Task AddOrUpdateCourse(Entities.Course course);
    Task AddRating(Entities.Rating rating);
    Task AddUserRating(Entities.UserCourseRating courseRating);
}
