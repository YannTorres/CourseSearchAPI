using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories.Roadmap;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CourseSearch.Infrastructure.DataAcess.Repositories;
internal class RoadmapRepository : IRoadmapReadOnlyRepository, IRoadmapWriteOnlyRepository, IRoadmapUpdateOnlyRepository
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
            .Include(r => r.Courses)
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

    public async Task<bool> RoadmapCourseExists(Domain.Entities.User user, Guid roadmapId, Guid courseId)
    {
        var roadmap = await _dbContext.Roadmaps
            .FirstOrDefaultAsync(r => r.Id == roadmapId && r.Creator.Id == user.Id);

        if (roadmap == null)
            return false;

        var course = await _dbContext.RoadmapCourses
            .FirstOrDefaultAsync(rc => rc.CourseId == courseId);

        if (course == null)
            return false;

        return true;
    }

    public async Task UpdateStatus(Guid roadmapId, Guid courseId, bool status)
    {
       var roadmapCourse = await _dbContext.RoadmapCourses
            .FirstOrDefaultAsync(x => x.RoadmapId == roadmapId && x.CourseId == courseId);

        if (roadmapCourse != null)
        {
            roadmapCourse.IsCompleted = status;
            await _dbContext.SaveChangesAsync();
        }

    }
}
