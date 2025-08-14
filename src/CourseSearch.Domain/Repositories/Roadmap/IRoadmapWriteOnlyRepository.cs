namespace CourseSearch.Domain.Repositories.Roadmap;
public interface IRoadmapWriteOnlyRepository
{
    Task Add(Entities.Roadmap roadmap);
}
