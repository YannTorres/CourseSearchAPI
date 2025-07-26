using CourseSearch.Domain.Entities;

namespace CourseSearch.Domain.Services.LoggedUser;
public interface ILoggedUser
{
    Task<User> Get();
}
