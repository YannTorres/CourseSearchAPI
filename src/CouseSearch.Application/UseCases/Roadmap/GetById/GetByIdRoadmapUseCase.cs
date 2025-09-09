using CourseSearch.Communication.Responses.Roadmap;
using CourseSearch.Domain.Extensions;
using CourseSearch.Domain.Repositories.Roadmap;
using CourseSearch.Domain.Services.LoggedUser;
using CourseSearch.Exception.ExceptionBase;

namespace CourseSearch.Application.UseCases.Roadmap.GetById;
public class GetByIdRoadmapUseCase : IGetByIdRoadmapUseCase
{
    private readonly IRoadmapReadOnlyRepository _roadmapRepository;
    private readonly ILoggedUser _loggedUser;
    public GetByIdRoadmapUseCase(IRoadmapReadOnlyRepository roadmapRepository, ILoggedUser loggedUser)
    {
        _roadmapRepository = roadmapRepository;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseRoadmapJson> Execute(Guid id)
    {
        var loggedUser = await _loggedUser.Get();
        var roadmap = await _roadmapRepository.GetById(id, loggedUser);

        if (roadmap == null)
            throw new NotFoundException("Roadmap não encontrado");

        return new ResponseRoadmapJson()
        {
            Id = roadmap.Id,
            RoadmapTitle = roadmap.Title,
            Description = roadmap.Description,
            RoadmapLevel = roadmap.ExperienceLevel.CourseLevelToString(),
            Steps = roadmap.StepsCount,
            Courses = roadmap.Courses.Select(c => new ResponseRoadmapCourseJson
            {
                Id = c.Course.Id,
                Title = c.Course.Title,
                Description = c.Course.Description,
                CourseLevels = c.Course.CourseLevels?.Select(cl => cl.CourseLevelToString()).ToList() ?? ["Nível Não Especificado"],
                DurationInMinutes = c.Course.DurationsInMinutes,
                IsCompleted = c.IsCompleted,
                RatingAverage = c.Course.Rating?.Average.ToString() ?? "Não Avaliado",
                StepOrder = c.StepOrder
            }).ToList()
        };
    }
}
