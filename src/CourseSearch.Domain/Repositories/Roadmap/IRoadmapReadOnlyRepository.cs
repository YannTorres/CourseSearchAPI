namespace CourseSearch.Domain.Repositories.Roadmap;
public interface IRoadmapReadOnlyRepository
{
    Task<Entities.Roadmap?> GetById(Guid id);
    Task<List<Entities.Roadmap>?> GetAll();
}
