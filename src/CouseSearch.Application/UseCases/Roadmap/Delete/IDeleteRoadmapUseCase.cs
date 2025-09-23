namespace CourseSearch.Application.UseCases.Roadmap.Delete;
public interface IDeleteRoadmapUseCase
{
    Task Execute(Guid id);
}
