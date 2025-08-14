using CourseSearch.Application.UseCases.Users.Register;
using CourseSearch.Communication.Requests.Roadmap;
using CourseSearch.Communication.Requests.Users;
using CourseSearch.Communication.Responses.Roadmap;
using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories;
using CourseSearch.Domain.Repositories.Course;
using CourseSearch.Domain.Repositories.Roadmap;
using CourseSearch.Domain.Services.IAModelService;
using CourseSearch.Domain.Services.LoggedUser;
using CourseSearch.Exception.ExceptionBase;

namespace CourseSearch.Application.UseCases.Roadmap.Create;
public class CreateRoadmapUseCase : ICreateRoadmapUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IAIModelService _aiModelService;
    private readonly ICourseReadOnlyRepository _courseRepository;
    private readonly IRoadmapWriteOnlyRepository _roadmapRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoadmapUseCase(ILoggedUser loggedUser,
        IAIModelService aiModelService,
        ICourseReadOnlyRepository courseRepository,
        IRoadmapWriteOnlyRepository roadmapRepository,
        IUnitOfWork unitOfWork
        )
    {
        _loggedUser = loggedUser;
        _aiModelService = aiModelService;
        _courseRepository = courseRepository;
        _roadmapRepository = roadmapRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseGenerateRoadmapJson> Execute(RequestGenerateRoadpmapJson request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request);

        // IA extrai os tópicos 
        var topics = await _aiModelService.ExtractTopicsFromObjectiveAsync(request);
        if (topics == null || topics.Count == 0) throw new NotFoundException("Não foi possivel analizar essa área de interesse, tente outra.");

        // Sistema busca os cursos candidatos 
        var candidateCourses = await _courseRepository.FindCoursesByTopics(topics);
        if (candidateCourses == null || candidateCourses.Count < 5) throw new NotFoundException("Não foram encontrados cursos dessa área de interesse em nosso catálogo.");

        // IA seleciona, ordena e enriquece os dados
        var aiSuggestion = await _aiModelService.SelectAndOrderCoursesAsync(request, candidateCourses);
        if (aiSuggestion == null) throw new NotFoundException("Não foi possível gerar o roadmap, tente novamente.");

        var newRoadmap = new Domain.Entities.Roadmap
        {
            Title = request.Objective,
            Description = "",
            CreatedAt = DateTime.UtcNow,
            Courses = new List<RoadmapCourse>(),
            Creator = loggedUser,
        };

        foreach (var step in aiSuggestion)
        {
            newRoadmap.Courses.Add(new RoadmapCourse
            {
                CourseId = Guid.Parse(step.CourseId),   
                RoadmapId = newRoadmap.Id,
                StepOrder = step.Order,
                CourseDescription = step.Description,
                CourseName = step.Title
            });
        }

        await _roadmapRepository.Add(newRoadmap);
        await _unitOfWork.Commit();

        return new ResponseGenerateRoadmapJson()
        {
            Title = newRoadmap.Title,
        };
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
