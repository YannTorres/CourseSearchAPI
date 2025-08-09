namespace CourseSearch.Domain.Repositories.Course;
public interface ICourseReadOnlyRepository
{
    Task<Entities.Course?> GetById(Guid id);
    IQueryable<Entities.Course> GetAll();
    Task<List<Entities.Course>?> FindCoursesByTopics(List<string> topics);
}
