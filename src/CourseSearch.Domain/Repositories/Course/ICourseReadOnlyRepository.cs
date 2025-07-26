namespace CourseSearch.Domain.Repositories.Course;
public interface ICourseReadOnlyRepository
{
    Task<Entities.Course?> GetByExternalIdAsync(string ExternalCourseId);
    Task<Entities.Course?> GetById(Guid id);
    IQueryable<Entities.Course> GetAll();
}
