using CourseSearch.Communication.Responses.Roadmap;
using CourseSearch.Domain.Repositories.Roadmap;
using CourseSearch.Domain.Services.LoggedUser;

namespace CourseSearch.Application.UseCases.Roadmap.GetAll;
public class GetAllRoadmapUseCase : IGetAllRoadmapUseCase
{
    private readonly IRoadmapReadOnlyRepository _roadmapRepository;
    private readonly ILoggedUser _loggedUser;
    public GetAllRoadmapUseCase(IRoadmapReadOnlyRepository roadmapRepository, ILoggedUser loggedUser)
    {
        _roadmapRepository = roadmapRepository;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseRoadmapsJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var roadmaps = await _roadmapRepository.GetAll(loggedUser);

        if (roadmaps == null || roadmaps.Count == 0) return new ResponseRoadmapsJson
        {
            Roadmaps = []
        };

        var roadmapsMap = roadmaps.Select(r => new ResponseShortRoadmapJson
        {
            Id = r.Id,
            Title = r.Title,
            Description = r.Description,
            StepsCount = r.StepsCount,
            CreatedAt = r.CreatedAt,
            CompletedCourses = r.Courses.Count(c => c.IsCompleted)
        }).ToList();

        return new ResponseRoadmapsJson
        {
            Roadmaps = roadmapsMap
        };
    }
}
