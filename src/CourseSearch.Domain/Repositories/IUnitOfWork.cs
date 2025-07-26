namespace CourseSearch.Domain.Repositories;
public interface IUnitOfWork
{
    public Task Commit();
}
