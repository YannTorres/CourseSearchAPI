using CourseSearch.Communication.Requests.Users;
using CourseSearch.Domain.Repositories;
using CourseSearch.Domain.Repositories.User;
using CourseSearch.Domain.Services.LoggedUser;
using CourseSearch.Exception.ExceptionBase;

namespace CourseSearch.Application.UseCases.Users.UpdateProfile;
public class UpdateUserProfileUseCase : IUpdateUserProfileUseCase
{
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateUserProfileUseCase(
        IUserUpdateOnlyRepository updateOnlyRepository,
        ILoggedUser loggedUser,
        IUserReadOnlyRepository readOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _updateOnlyRepository = updateOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request);

        var user = await _updateOnlyRepository.GetById(loggedUser.Id); 

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        _updateOnlyRepository.Update(user);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestUpdateUserJson request)
    {
        var validator = new UpdateUserValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
