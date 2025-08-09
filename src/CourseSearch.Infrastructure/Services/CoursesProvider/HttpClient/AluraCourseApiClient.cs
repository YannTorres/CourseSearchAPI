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

    // Método para buscar a lista de todos os slugs de cursos
    public async Task<IEnumerable<AluraCourseResponseDTO>> GetCourseListAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<AluraCourseResponseDTO>>("api/cursos");
            return response ?? Enumerable.Empty<AluraCourseResponseDTO>();
        }
        catch (Exception)
        {
            // Logar o erro
            return Enumerable.Empty<AluraCourseResponseDTO>();
        }
    }

    // Método para buscar os detalhes de um curso específico pelo slug
    public async Task<AluraCourseDetailsDto?> GetCourseDetailsAsync(string slug)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<AluraCourseDetailsDto>($"api/curso-{slug}");
        }
        catch (Exception)
        {
            // Logar o erro, o curso pode não existir mais
            return null;
        }
    }
}
