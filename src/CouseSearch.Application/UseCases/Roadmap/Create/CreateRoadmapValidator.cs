using CourseSearch.Communication.Requests.Roadmap;
using FluentValidation;

namespace CourseSearch.Application.UseCases.Roadmap.Create;
public class CreateRoadmapValidator : AbstractValidator<RequestGenerateRoadpmapJson>
{
    public CreateRoadmapValidator()
    {
        RuleFor(request => request.Objective)
            .NotEmpty().WithMessage("O objetivo não pode estar vazio.")
            .MaximumLength(500).WithMessage("O objetivo não pode ter mais de 500 caracteres.");
        RuleFor(request => request.AreaOfInterest)
            .NotEmpty().WithMessage("A área de interesse não pode estar vazia.")
            .MaximumLength(100).WithMessage("A área de interesse não pode ter mais de 100 caracteres.");
        RuleFor(request => request.ExperienceLevel)
            .IsInEnum().WithMessage("O nível de experiência não é valido.");
    }
}
