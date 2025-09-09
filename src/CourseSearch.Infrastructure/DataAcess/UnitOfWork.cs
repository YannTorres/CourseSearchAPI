using CourseSearch.Domain.Repositories;
using EFCore.BulkExtensions;

namespace CourseSearch.Infrastructure.DataAcess;
internal class UnitOfWork : IUnitOfWork
{
    private readonly CourseSearchDbContext _dbcontext;

    public UnitOfWork(CourseSearchDbContext dbContext)
    {
        _dbcontext = dbContext;
    }
    public async Task Commit()
    {
        await _dbcontext.SaveChangesAsync();
    }
}
