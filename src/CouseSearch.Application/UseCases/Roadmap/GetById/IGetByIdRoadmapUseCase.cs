using CourseSearch.Communication.Responses.Roadmap;

namespace CourseSearch.Application.UseCases.Roadmap.GetById;
public interface IGetByIdRoadmapUseCase
{
    Task<ResponseRoadmapJson> Execute(Guid id);
}
