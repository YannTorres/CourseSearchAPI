namespace CourseSearch.Domain.Repositories.Course;
public interface ICourseWriteOnlyRepository
{
    Task AddOrUpdateCourse(Entities.Course course, string plataformName);
}
