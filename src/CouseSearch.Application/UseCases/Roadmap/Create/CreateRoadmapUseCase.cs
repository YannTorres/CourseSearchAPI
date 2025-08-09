using CourseSearch.Application.UseCases.Users.Register;
using CourseSearch.Communication.Requests.Roadmap;
using CourseSearch.Communication.Requests.Users;
using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories.Course;
using CourseSearch.Domain.Services.IAModelService;
using CourseSearch.Domain.Services.LoggedUser;
using CourseSearch.Exception.ExceptionBase;

namespace CourseSearch.Application.UseCases.Roadmap.Create;
public class CreateRoadmapUseCase : ICreateRoadmapUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IAIModelService _aiModelService;
    private readonly ICourseReadOnlyRepository _courseRepository;

    public CreateRoadmapUseCase(ILoggedUser loggedUser,
        IAIModelService aiModelService,
        ICourseReadOnlyRepository courseRepository
        )
    {
        _loggedUser = loggedUser;
        _aiModelService = aiModelService;
        _courseRepository = courseRepository;
    }
    public async Task<Domain.Entities.Roadmap?> Execute(RequestGenerateRoadpmapJson request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request);

        // --- PASSO 1: IA extrai os tópicos ---
        var topics = await _aiModelService.ExtractTopicsFromObjectiveAsync(request);
        if (topics == null || topics.Count == 0) return null;

        // --- PASSO 2: Seu sistema busca os cursos candidatos ---
        var candidateCourses = await _courseRepository.FindCoursesByTopics(topics);
        if (candidateCourses == null || !candidateCourses.Any()) return null;

        // --- PASSO 3: IA seleciona, ordena e enriquece os dados ---
        var aiSuggestion = await _aiModelService.SelectAndOrderCoursesAsync(request, candidateCourses);
        if (aiSuggestion == null) return null;

        // --- Persistência no Banco de Dados ---
        // Cria a entidade principal do Roadmap

        var newRoadmap = new Domain.Entities.Roadmap();

        foreach (var step in aiSuggestion.Roadmap)
        {
            var itemRoadmap = new Domain.Entities.Roadmap
            {
                Title = step.Title,
                Description = step.Description,
                CreatorUserId = loggedUser.Id,
                UpdatedAt = DateTime.UtcNow
            };



            if (string.IsNullOrWhiteSpace(step.Title))
            {
                throw new ErrorOnValidationException(new List<string> { "Invalid course data in AI suggestion." });
            }
        }

        // Adiciona os cursos ao roadmap com a ordem e justificativa
        //foreach (var step in aiSuggestion.Steps)
        //{
        //    var course = candidateCourses.FirstOrDefault(c => c.Id == step.CourseId);
        //    if (course != null)
        //    {
        //        newRoadmap.Courses.Add(new RoadmapCourse
        //        {
        //            Course = course,
        //            StepOrder = step.Order,
        //        });
        //    }
        //}

        // Salva tudo no banco
        //await _roadmapRepository.AddAsync(newRoadmap);
        //await _roadmapRepository.SaveChangesAsync();

        return newRoadmap;
    }

    private static void Validate(RequestGenerateRoadpmapJson request)
    {
        var result = new CreateRoadmapValidator().Validate(request);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }

    }
}
