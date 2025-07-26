using CourseSearch.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
