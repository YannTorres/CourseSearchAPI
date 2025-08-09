using CourseSearch.Communication.Requests.Users;
using FluentValidation;

namespace CourseSearch.Application.UseCases.Users.Register;
public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.FirstName).NotEmpty().WithMessage("O nome não pode estar vazio.");
        RuleFor(user => user.LastName).NotEmpty().WithMessage("O Sobrenome não pode estar vazio.");
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("O Email não pode estar vazio.")
            .EmailAddress().When(user => string.IsNullOrEmpty(user.Email) == false, ApplyConditionTo.CurrentValidator)
            .WithMessage("O Email não está em um formato válido.");
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());

    }
}
