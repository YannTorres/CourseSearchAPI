
using CourseSearch.Domain.Repositories;
using CourseSearch.Domain.Repositories.Roadmap;
using CourseSearch.Domain.Services.LoggedUser;
using CourseSearch.Exception.ExceptionBase;

namespace CourseSearch.Application.UseCases.Roadmap.Delete;
public class DeleteRoadmapUseCase : IDeleteRoadmapUseCase
{
    private readonly IRoadmapWriteOnlyRepository _writeOnlyRepository;
    private readonly IRoadmapReadOnlyRepository _readOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteRoadmapUseCase(
        IRoadmapWriteOnlyRepository writeOnlyRepository,
        IRoadmapReadOnlyRepository readOnlyRepository,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork
        )
    {
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(Guid id)
    {
        var loggedUser = await _loggedUser.Get();

        var roadmap = await _readOnlyRepository.GetById(id, loggedUser);

        if (roadmap == null)
            throw new NotFoundException("Roadmap não encontrado");

        await _writeOnlyRepository.Delete(id, loggedUser);
        await _unitOfWork.Commit();
    }
}
