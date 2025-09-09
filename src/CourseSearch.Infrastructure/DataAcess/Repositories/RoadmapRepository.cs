using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories.Roadmap;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<Roadmap>?> GetAll(Domain.Entities.User user)
    {
        return await _dbContext.Roadmaps
            .Where(r => r.Creator.Id == user.Id)
            .ToListAsync();
    }

    public async Task<Roadmap?> GetById(Guid id, Domain.Entities.User user)
    {
        return await _dbContext.Roadmaps
            .Include(r => r.Courses.OrderBy(rc => rc.StepOrder))
                .ThenInclude(rc => rc.Course)
                .ThenInclude(rc => rc.Rating)
            .Include(r => r.Creator)
            .FirstOrDefaultAsync(r => r.Id == id && r.Creator.Id == user.Id);
    }
}
