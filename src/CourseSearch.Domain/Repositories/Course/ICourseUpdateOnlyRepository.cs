using CourseSearch.Domain.Entities;

namespace CourseSearch.Domain.Repositories.Course;
public interface ICourseUpdateOnlyRepository
{
    void Update(Entities.Course course);
    Task<Entities.Course?> GetById(Guid id);
    void UpdateRating(Rating course);
    void UpdateUserRating(UserCourseRating courseRating);
}
