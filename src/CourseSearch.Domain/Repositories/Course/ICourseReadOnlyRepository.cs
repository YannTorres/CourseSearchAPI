using CourseSearch.Domain.Entities;

namespace CourseSearch.Domain.Repositories.Course;
public interface ICourseReadOnlyRepository
{
    Task<Entities.Course?> GetById(Guid id);
    IQueryable<Entities.Course> GetAll();
    Task<Entities.Course?> GetByExternalIdAsync(string ExternalCourseId);
    Task<List<Entities.Course>> GetCourseBySharedTags(List<string> tagNames, Guid excludeCourseId);
    Task<List<Entities.Course>?> FindCoursesByTopics(List<string> topics);
    Task<UserCourseRating?> GetUserRating(Guid courseId, Guid userId);
}
