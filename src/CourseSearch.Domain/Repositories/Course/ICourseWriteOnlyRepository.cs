namespace CourseSearch.Domain.Repositories.Course;
public interface ICourseWriteOnlyRepository
{
    Task Add(Entities.Course course);
    Task Delete(Entities.Course course);
}
