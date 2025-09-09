using CourseSearch.Communication.Responses.Roadmap;

namespace CourseSearch.Application.UseCases.Roadmap.GetAll;
public interface IGetAllRoadmapUseCase
{
    public Task<ResponseRoadmapsJson> Execute();
}
