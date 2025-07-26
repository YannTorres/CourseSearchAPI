using CourseSearch.Communication.Requests.Users;

namespace CourseSearch.Application.UseCases.Users.UpdateProfile;
public interface IUpdateUserProfileUseCase
{
    Task Execute(RequestUpdateUserJson request);
}
