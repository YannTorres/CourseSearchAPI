using CourseSearch.Communication.Requests.Users;
using FluentValidation;

namespace CourseSearch.Application.UseCases.Users.UpdateProfile;
internal class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(user => user.FirstName).NotEmpty().WithMessage("Primeiro nome vazio");
        RuleFor(user => user.LastName).NotEmpty().WithMessage("Ultimo nom vazio");
    }
}
