using CourseSearch.Communication.Requests.Users;
using CourseSearch.Communication.Responses.Users;
using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories;
using CourseSearch.Domain.Repositories.User;
using CourseSearch.Domain.Security.Cryptography;
using CourseSearch.Domain.Security.Tokens;
using CourseSearch.Exception.ExceptionBase;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace CourseSearch.Application.UseCases.Users.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository _repositoryWriteOnly;
    private readonly IUserReadOnlyRepository _repositoryReadOnly;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passswordEncripter;
    private readonly IAcessTokenGenerator _tokenGenerator;
    public RegisterUserUseCase(
        IUserWriteOnlyRepository repositoryWriteOnly,
        IUserReadOnlyRepository repositoryReadOnly,
        IUnitOfWork unitOfWork,
        IAcessTokenGenerator tokenGenerator,
        IPasswordEncripter passswordEncripter)
    {
        _unitOfWork = unitOfWork;
        _tokenGenerator = tokenGenerator;
        _repositoryWriteOnly = repositoryWriteOnly;
        _repositoryReadOnly = repositoryReadOnly;
        _passswordEncripter = passswordEncripter;
    }
    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = new User()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
        };
        user.Password = _passswordEncripter.Encript(request.Password);

        await _repositoryWriteOnly.Add(user);
        await _unitOfWork.Commit();

        return new ResponseRegisterUserJson()
        {
            User = new Communication.Extension.UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Email = user.Email
            },
            Token = _tokenGenerator.Generate(user)
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var result = new RegisterUserValidator().Validate(request);

        var existEmail = await _repositoryReadOnly.ExistUserWithEmail(request.Email);

        if (existEmail)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, "Este Email já foi cadastrado."));
        }

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }

    }
}
