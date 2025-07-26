namespace CourseSearch.Domain.Repositories.Course;
public interface ICourseUpdateOnlyRepository
{
    Task<Entities.Course?> GetById(Guid id);
    void Update(Entities.Course course);
}
