using CourseSearch.Communication.Requests.Roadmap;
using CourseSearch.Domain.Dtos.IASuggestions;
using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Services.IAModelService;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CourseSearch.Infrastructure.Services.IAModelService;
public class GeminiModelService : IAIModelService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;
    private const string GeminiModel = "gemini-2.5-flash-lite";
    public GeminiModelService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _apiKey = _configuration["Gemini:ApiKey"]
            ?? throw new ArgumentNullException(nameof(configuration), "A chave da API do Gemini não foi encontrada. Configure-a via User Secrets.");
    }
    public async Task<List<string>> ExtractTopicsFromObjectiveAsync(RequestGenerateRoadpmapJson request)
    {
        var prompt = $"""
            Você é um especialista em planejamento de carreiras de tecnologia.
            Um usuário com nível de experiência '{request.ExperienceLevel}' tem o seguinte objetivo: '{request.Objective}' na área de '{request.AreaOfInterest}'.
           Analise este objetivo e retorne uma lista dos tópicos técnicos, habilidades e tecnologias essenciais que ele precisa aprender para alcançar seu objetivo.
           Sua resposta DEVE SER APENAS um JSON array de strings em INGLÊS, sem nenhuma explicação, evite usar palavras compostas seja direto na resposta, ex: dotnet, ccharp, sql, e etc.
           Exemplo de resposta esperada: ['topico-1', 'topico-2', 'topico-3'] (Importante dividir as tags com '-') (Faça no Mínimo 100 tags relacionadas)
        """;

        var responseText = await GenerateContentAsync(prompt);

        if (string.IsNullOrEmpty(responseText))
        {
            return [];
        }

        try
        {
            var cleanJson = responseText.Trim().Replace("```json", "").Replace("```", "");
            return JsonSerializer.Deserialize<List<string>>(cleanJson) ?? [];
        }
        catch (JsonException)
        {
            // Retornar uma lista vazia para não quebrar o sistema.
            return [];
        }
    }

    public async Task<List<AISuggestionContentDto>?> SelectAndOrderCoursesAsync(RequestGenerateRoadpmapJson request, IEnumerable<Course> candidateCourses)
    {
        var simplifiedCourses = candidateCourses.Select(c => new 
        {
            id = c.Id,
            title = c.Title,
            description = c.Description,
            tags = c.Tags.Select(t => t.Name)
        });
        var coursesJson = JsonSerializer.Serialize(simplifiedCourses);

        var prompt = $"""
            Você é um especialista em criar trilhas de aprendizado (roadmaps) personalizadas.
            Um usuário com nível '{request.ExperienceLevel}' quer alcançar o objetivo: '{request.Objective}'.
            Eu tenho os seguintes cursos disponíveis no meu banco de dados:
            {coursesJson}

            Sua tarefa é agir como um filtro e sequenciador inteligente.
            1.  Selecione APENAS da lista fornecida os cursos mais relevantes para o objetivo do usuário.
            3.  A Lista deve ter no máximo 20 cursos.
            2.  Ordene os cursos selecionados em uma sequência lógica de aprendizado.

            Sua resposta DEVE SER APENAS um objeto JSON válido, sem nenhuma outra formatação, texto ou explicação, contento estas propriedades:
              "CourseId": "ID do curso que está disponível no meu banco de dados.",
              "Title": "Título do curso que está disponível no meu banco de dados.",
              "Description": "Descrição do curso que está disponível no meu banco de dados. se estiver vazio crie uma descrição curta",
              "Order": número inteiro indicando a ordem do curso na trilha de aprendizado, começando em 1 para o primeiro curso.
        """;

        var responseText = await GenerateContentAsync(prompt);
        if (string.IsNullOrEmpty(responseText))
        {
            return null;
        }

        try
        {
            var cleanJson = responseText.Trim().Replace("```json", "").Replace("```", "");
            return JsonSerializer.Deserialize<List<AISuggestionContentDto>?>(cleanJson);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private async Task<string?> GenerateContentAsync(string prompt)
    {
        var apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{GeminiModel}:generateContent?key={_apiKey}";

        var requestBody = new GeminiRequest(
            Contents: [new Content([new Part(prompt)])],
            GenerationConfig: new GenerationConfig(1, 8192, 0.5m) // CandidateCount, Max Tokens, Temperature
        );

        var jsonBody = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsJsonAsync($"{apiUrl}", requestBody);

        if (!response.IsSuccessStatusCode)
        {
            // Logar o erro
            return null;
        }

        var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();

        return geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;
    }
}
