using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories.Roadmap;
using System.Data.Entity;

namespace CourseSearch.Infrastructure.DataAcess.Repositories;
internal class RoadmapRepository : IRoadmapReadOnlyRepository, IRoadmapWriteOnlyRepository
{
    private readonly CourseSearchDbContext _dbContext;
    public RoadmapRepository(CourseSearchDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(Roadmap roadmap)
    {
        _dbContext.Attach(roadmap.Creator);

        await _dbContext.Roadmaps.AddAsync(roadmap);
    }

    public async Task<List<Roadmap>?> GetAll()
    {
        return await _dbContext.Roadmaps
            .Include(r => r.Courses)
            .ToListAsync();
    }

    public Task<Roadmap?> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}
