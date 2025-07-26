using CourseSearch.Communication.Requests.Users;

namespace CourseSearch.Application.UseCases.Users.ChangePassword;
public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}
