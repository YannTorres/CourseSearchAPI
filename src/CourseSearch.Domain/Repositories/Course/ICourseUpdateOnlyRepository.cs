namespace CourseSearch.Domain.Repositories.Course;
public interface ICourseUpdateOnlyRepository
{
    Task<Entities.Course?> GetByExternalIdAsync(string ExternalCourseId);
    void Update(Entities.Course course);
}
