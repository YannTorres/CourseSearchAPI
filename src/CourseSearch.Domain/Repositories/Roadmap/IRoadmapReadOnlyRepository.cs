namespace CourseSearch.Domain.Repositories.Roadmap;
public interface IRoadmapReadOnlyRepository
{
    Task<Entities.Roadmap?> GetById(Guid id, Entities.User user);
    Task<List<Entities.Roadmap>?> GetAll(Entities.User user);
}
