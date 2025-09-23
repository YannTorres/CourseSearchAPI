using CourseSearch.Communication.Requests.Courses;
using FluentValidation;

namespace CourseSearch.Application.UseCases.Rating.AddRating;
public class AddRatingValidator : AbstractValidator<RequestAddRatingJson>
{
    public AddRatingValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("A Avaliação deve ser estar no intervalo de 1 a 5");
    }
}
