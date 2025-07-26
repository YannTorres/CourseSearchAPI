
using CourseSearch.Domain.Repositories;
using CourseSearch.Domain.Repositories.User;
using CourseSearch.Domain.Services.LoggedUser;

namespace CourseSearch.Application.UseCases.Users.Delete;
public class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteUserUseCase(ILoggedUser loggedUser, IUserWriteOnlyRepository writeOnlyRepository, IUnitOfWork unitOfWork)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute()
    {
        var user = await _loggedUser.Get();

        await _writeOnlyRepository.Delete(user);

        await _unitOfWork.Commit();
    }
}
