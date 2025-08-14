using CourseSearch.Domain.Dtos.AluraCourses;
using CourseSearch.Domain.Services.CourseProvider.HttpClient;
using System.Net.Http.Json;

namespace CourseSearch.Infrastructure.Services.CoursesProvider.HttpClient;
public class AluraCourseApiClient : IAluraCourseApiClient
{
    private readonly System.Net.Http.HttpClient _httpClient;

    public AluraCourseApiClient(System.Net.Http.HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<IEnumerable<AluraCourseResponseDTO>> GetCourseListAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<AluraCourseResponseDTO>>("api/cursos");
            return response ?? Enumerable.Empty<AluraCourseResponseDTO>();
        }
        catch (Exception)
        {
            return Enumerable.Empty<AluraCourseResponseDTO>();
        }
    }
    public async Task<AluraCourseDetailsDto?> GetCourseDetailsAsync(string slug)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<AluraCourseDetailsDto>($"api/curso-{slug}");
        }
        catch (Exception)
        {
            return null;
        }
    }
}
