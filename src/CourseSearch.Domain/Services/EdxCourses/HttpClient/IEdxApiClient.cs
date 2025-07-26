using CourseSearch.Domain.Dtos.EdxCourses;

namespace CourseSearch.Domain.Services.EdxCourses.HttpClient;
public interface IEdxApiClient
{
    Task<EdxCourseResponseDto?> GetCoursesByPageAsync(int page, int pageSize);
}
