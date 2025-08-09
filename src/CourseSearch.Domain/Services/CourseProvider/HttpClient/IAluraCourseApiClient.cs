using CourseSearch.Domain.Dtos.AluraCourses;

namespace CourseSearch.Domain.Services.CourseProvider.HttpClient;
public interface IAluraCourseApiClient
{
    Task<IEnumerable<AluraCourseResponseDTO>> GetCourseListAsync();

    Task<AluraCourseDetailsDto?> GetCourseDetailsAsync(string slug);
}
