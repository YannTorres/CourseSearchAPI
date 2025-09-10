using CourseSearch.Communication.Requests.Roadmap;
using FluentValidation;

namespace CourseSearch.Application.UseCases.Roadmap.UpdateStatus;
public class UpdateRoadmapStatusValidator : AbstractValidator<RequestUpdateRoadmapStatus>
{
    public UpdateRoadmapStatusValidator()
    {
        RuleFor(r => r.IsCompleted).NotNull().WithMessage("O status do roadmap não pode ser nulo");
    }
}
