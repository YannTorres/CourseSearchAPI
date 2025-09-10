using CourseSearch.Application.UseCases.Users.UpdateProfile;
using CourseSearch.Communication.Requests.Roadmap;
using CourseSearch.Domain.Repositories;
using CourseSearch.Domain.Repositories.Roadmap;
using CourseSearch.Domain.Services.LoggedUser;
using CourseSearch.Exception.ExceptionBase;

namespace CourseSearch.Application.UseCases.Roadmap.UpdateStatus;
public class UpdateRoadmapStatusUseCase : IUpdateRoadmapStatusUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRoadmapUpdateOnlyRepository _updateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateRoadmapStatusUseCase(
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork,
        IRoadmapUpdateOnlyRepository updateOnlyRepository
        )
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _updateOnlyRepository = updateOnlyRepository;
    }
    public async Task Execute(Guid roadmapId, Guid couseId, RequestUpdateRoadmapStatus request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var exists = await _updateOnlyRepository.RoadmapCourseExists(loggedUser, roadmapId, couseId);
        
        if (exists == false)
            throw new NotFoundException("Recomendação e/ou curso não encontrado");

        await _updateOnlyRepository.UpdateStatus(roadmapId, couseId, request.IsCompleted);
        await _unitOfWork.Commit();
    }
    private static void Validate(RequestUpdateRoadmapStatus request)
    {
        var validator = new UpdateRoadmapStatusValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
