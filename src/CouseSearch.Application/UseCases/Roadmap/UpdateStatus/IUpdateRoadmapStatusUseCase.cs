using CourseSearch.Communication.Requests.Roadmap;
using CourseSearch.Communication.Responses.Roadmap;

namespace CourseSearch.Application.UseCases.Roadmap.UpdateStatus;
public interface IUpdateRoadmapStatusUseCase
{
    Task Execute(Guid roadmapId, Guid couseId, RequestUpdateRoadmapStatus request);
}
