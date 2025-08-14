using CourseSearch.Communication.Requests.Roadmap;
using CourseSearch.Communication.Responses.Roadmap;

namespace CourseSearch.Application.UseCases.Roadmap.Create;
public interface ICreateRoadmapUseCase
{
    Task<ResponseGenerateRoadmapJson> Execute(RequestGenerateRoadpmapJson request);
}
