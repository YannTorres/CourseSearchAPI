using CourseSearch.Communication.Extension;
using CourseSearch.Communication.Requests.Users;
using CourseSearch.Communication.Responses.Users;
using CourseSearch.Domain.Repositories.User;
using CourseSearch.Domain.Security.Cryptography;
using CourseSearch.Domain.Security.Tokens;
using CourseSearch.Exception.ExceptionBase;

namespace CourseSearch.Application.UseCases.Login;
public class LoginUseCase : ILoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IPasswordEncripter _encripter;
    private readonly IAcessTokenGenerator _acessTokenGenerator;
    public LoginUseCase(
        IUserReadOnlyRepository repository,
        IPasswordEncripter encripter,
        IAcessTokenGenerator acessTokenGenerator)
    {
        _acessTokenGenerator = acessTokenGenerator;
        _repository = repository;
        _encripter = encripter;
    }
    public async Task<ResponseLoginUserJson> Execute(RequestLoginJson request)
    {
        await Task.Delay(TimeSpan.FromSeconds(2));

        var user = await _repository.GetUserByEmail(request.Email);

        if (user == null)
        {
            throw new UnauthorizedException();
        }

        var passwordMatch = _encripter.Verify(request.Password, user.Password);

        if (passwordMatch == false)
        {
            throw new UnauthorizedException();
        }

        return new ResponseLoginUserJson
        {
            User = new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Email = user.Email
            },
            Token = _acessTokenGenerator.Generate(user)
        };
    }
}
