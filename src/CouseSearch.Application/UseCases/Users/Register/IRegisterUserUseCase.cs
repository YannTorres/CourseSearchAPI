using CourseSearch.Communication.Requests.Users;
using CourseSearch.Communication.Responses.Users;

namespace CourseSearch.Application.UseCases.Users.Register;
public interface IRegisterUserUseCase
{
    public Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request);
}
