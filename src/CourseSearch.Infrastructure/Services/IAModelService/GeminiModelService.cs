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
    private const string GeminiModelRoadmap = "gemini-2.5-pro";
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
           Um usuário com nível de experiência '{request.ExperienceLevel}' tem o seguinte objetivo: '{request.Objective}' e ele tem interesses nas seguintes técnologias '{request.PriorityTechs}'.
           Analise este objetivo e retorne uma lista dos tópicos técnicos, habilidades e tecnologias essenciais que ele precisa aprender para alcançar seu objetivo.
           IMPORANTE: Foque em tópicos relacionados com as tecnologias que ele quer aprender.
           Sua resposta DEVE SER APENAS um JSON array de strings em PORTUGUÊS DO BRASIL, sem nenhuma explicação, NÃO USE palavras compostas E seja direto na resposta, ex: dotnet, ccharp, sql, e etc.
           Exemplo de resposta esperada: ["Topico 1", "Topico 2", "Topico 3"] (Importante começar com maiúscula e separar com espaços) (Faça apenas 10 tags relacionadas no MAXIMO, podendo ter menos)
        """;

        var responseText = await GenerateContentAsync(prompt, GeminiModelRoadmap);

        if (string.IsNullOrEmpty(responseText))
        {
            return [];
        }

        try
        {
            var cleanJson = responseText;

            if (cleanJson.Contains("```"))
                cleanJson = responseText.Trim().Replace("```json", "").Replace("```", "");
            return JsonSerializer.Deserialize<List<string>>(cleanJson) ?? [];
        }
        catch (JsonException)
        {
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
            Um usuário com nível '{request.ExperienceLevel}' quer alcançar o objetivo: '{request.Objective}'
            e tem vontade de aprender as seguintes tecnologias '{request.PriorityTechs}'.
            Eu tenho os seguintes cursos disponíveis no meu banco de dados:
            {coursesJson}

            Sua tarefa é agir como um filtro e sequenciador inteligente.
            1.  Selecione APENAS da lista fornecida os cursos mais relevantes para o objetivo do usuário.
            2.  A Lista deve ter no máximo 20 cursos.
            3.  Ordene os cursos selecionados em uma sequência lógica de aprendizado.
            4.  Selecione os cursos que melhor de alinham com as tecnologias de interesse do usuário, se não encontrado nenhum selecione os que o mercado de trabalho mais procura.

            Sua resposta DEVE SER APENAS um objeto JSON válido, sem nenhuma outra formatação, texto ou explicação, contento estas propriedades:
              "CourseId": "ID do curso que está disponível no meu banco de dados.",
              "Title": "Título do curso que está disponível no meu banco de dados.",
              "Description": "Descrição do curso que está disponível no meu banco de dados. se estiver vazio crie uma descrição curta",
              "Order": número inteiro indicando a ordem do curso na trilha de aprendizado, começando em 1 para o primeiro curso.
        """;

        var responseText = await GenerateContentAsync(prompt, GeminiModelRoadmap);
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

    public async Task<List<Tag>> GenerateTagsForCourseAsync(Guid courseId, string courseTitle, string courseDescription)
    {
        var prompt = $@"
        Analise o título e a descrição de um curso online e gere de 5 a 10 tags relevantes.
        As tags devem cobrir tecnologias, conceitos, nível de dificuldade (iniciante, intermediário, avançado) e área de atuação.
        Retorne APENAS as tags em uma única linha, separadas por vírgula.

        Exemplo de entrada:
        Título: 'Curso de C# e .NET: Criando uma API REST do zero'
        Descrição: 'Aprenda a construir APIs robustas com C#, ASP.NET Core. Abordaremos controllers, models, Entity Framework e boas práticas de arquitetura.'
        Exemplo de saída:
        C#, .NET, API, REST, ASP.NET Core, Entity Framework, Backend, Intermediário

        ---

        Entrada Real:
        Título: '{courseTitle}'
        Descrição: '{courseDescription}'
        Saída:
        ";

        try
        {

            var responseText = await GenerateContentAsync(prompt, GeminiModel);
            
            if (!string.IsNullOrEmpty(responseText))
            {
                return responseText.Split(',')
                                    .Select(tagName => tagName.Trim())
                                    .Where(tagName => !string.IsNullOrEmpty(tagName))
                                    .Select(tagName => new Tag
                                    {
                                        Name = tagName,
                                        CourseId = courseId 
                                    }).ToList();
            }

            return [];
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<string?> GenerateContentAsync(string prompt, string geminiModel)
    {
        var apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{geminiModel}:generateContent?key={_apiKey}";

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
