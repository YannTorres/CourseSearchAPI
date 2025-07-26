using CourseSearch.Communication.Responses.Users;

namespace CourseSearch.Application.UseCases.Users.GetProfile;
public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}
