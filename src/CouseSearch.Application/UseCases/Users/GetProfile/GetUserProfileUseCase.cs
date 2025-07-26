using CourseSearch.Communication.Responses.Users;
using CourseSearch.Domain.Services.LoggedUser;

namespace CourseSearch.Application.UseCases.Users.GetProfile;
public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;
    public GetUserProfileUseCase(ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
    }
    public async Task<ResponseUserProfileJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        return new ResponseUserProfileJson
        {
            Email = loggedUser.Email,
            FirstName = loggedUser.FirstName,
            LastName = loggedUser.LastName
        };
    }
}
