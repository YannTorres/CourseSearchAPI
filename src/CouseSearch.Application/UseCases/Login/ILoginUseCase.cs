using CourseSearch.Communication.Requests.Users;
using CourseSearch.Communication.Responses.Users;

namespace CourseSearch.Application.UseCases.Login;
public interface ILoginUseCase
{
    Task<ResponseLoginUserJson> Execute(RequestLoginJson request);
}
