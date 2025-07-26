using CourseSearch.Communication.Requests.Users;
using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories;
using CourseSearch.Domain.Repositories.User;
using CourseSearch.Domain.Security.Cryptography;
using CourseSearch.Domain.Services.LoggedUser;
using CourseSearch.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CourseSearch.Application.UseCases.Users.ChangePassword;
public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    public ChangePasswordUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IPasswordEncripter passwordEncripter)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request, loggedUser);

        var user = await _repository.GetById(loggedUser.Id);
        user.Password = _passwordEncripter.Encript(request.NewPassword);

        _repository.Update(user);

        await _unitOfWork.Commit();

    }

    private void Validate(RequestChangePasswordJson request, User loggedUser)
    {
        var validator = new ChangePasswordValidator();

        var result = validator.Validate(request);

        var passwordMatch = _passwordEncripter.Verify(request.OldPassword, loggedUser.Password);

        if (passwordMatch == false)
            result.Errors.Add(new ValidationFailure(string.Empty, "A senha que você digitou é diferente da senha atual."));

        if (result.IsValid == false)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}
