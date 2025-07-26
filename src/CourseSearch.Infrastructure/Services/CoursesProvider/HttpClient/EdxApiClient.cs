using CourseSearch.Domain.Dtos.EdxCourses;
using CourseSearch.Domain.Services.EdxCourses.HttpClient;
using System.Net.Http.Json;

namespace CourseSearch.Infrastructure.Services.EdxCourses.HttpClient;
public class EdxApiClient : IEdxApiClient
{
    private readonly System.Net.Http.HttpClient _httpClient;

    public EdxApiClient(System.Net.Http.HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<EdxCourseResponseDto?> GetCoursesByPageAsync(int page = 1, int pageSize = 10)
    {
        var endpoint = $"/api/courses/v1/courses/?page={page}&page_size={pageSize}";
        try
        {
            var response = await _httpClient.GetFromJsonAsync<EdxCourseResponseDto>(endpoint);
            return response;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Erro ao buscar a página {page}: {e.Message}");
            return null;
        }
    }
}
