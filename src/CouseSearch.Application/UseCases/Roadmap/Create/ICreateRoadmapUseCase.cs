using CourseSearch.Communication.Requests.Roadmap;

namespace CourseSearch.Application.UseCases.Roadmap.Create;
public interface ICreateRoadmapUseCase
{
    Task<Domain.Entities.Roadmap?> Execute(RequestGenerateRoadpmapJson request);
}
