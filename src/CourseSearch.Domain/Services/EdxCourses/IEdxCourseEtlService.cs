using CourseSearch.Domain.Dtos.EdxCourses;

namespace CourseSearch.Domain.Services.EdxCourses;
public interface IEdxCourseEtlService
{
    Task SynchronizeCourseFromDtoAsync(EdxCourseDto edxCourseDto);
}
