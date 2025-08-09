using CourseSearch.Domain.Dtos.EdxCourses;
using CourseSearch.Domain.Dtos.MicrosoftLearningCourses;
using CourseSearch.Domain.Services.CourseProvider.HttpClient;
using System.Net.Http.Json;

namespace CourseSearch.Infrastructure.Services.EdxCourses.HttpClient;
internal class MicrosoftLearnApiClient : IMicrosoftLearnApiClient
{
    private readonly System.Net.Http.HttpClient _httpClient;

    public MicrosoftLearnApiClient(System.Net.Http.HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<MicrosoftCourseResponseDTO?> GetFullCatalogAsync()
    {
        var endpoint = $"/api/catalog?locale=pt-br&type=modules";
        try
        {
            var response = await _httpClient.GetFromJsonAsync<MicrosoftCourseResponseDTO>(endpoint);
            return response;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Erro ao buscar: {e.Message}");
            return null;
        }
    }
}
