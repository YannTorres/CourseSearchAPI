using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories.Course;
using Microsoft.EntityFrameworkCore;

namespace CourseSearch.Infrastructure.DataAcess.Repositories;
internal class CourseRepository : ICourseReadOnlyRepository, ICourseWriteOnlyRepository, ICourseUpdateOnlyRepository
{
    private readonly CourseSearchDbContext _dbcontext;

    public CourseRepository(CourseSearchDbContext dbContext)
    {
        _dbcontext = dbContext;
    }
    public async Task Add(Course course)
    {
        await _dbcontext.Courses.AddAsync(course);
    }

    public Task Delete(Course course)
    {
        throw new NotImplementedException();
    }

    public async Task<Course?> GetByExternalIdAsync(string edxCourseId)
    {
        return null;
    }

    async Task<Course?> ICourseUpdateOnlyRepository.GetById(Guid id)
    {
        return await _dbcontext.Courses
            .Include(c => c.Tags)
            .Include(c => c.Platform)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    async Task<Course?> ICourseReadOnlyRepository.GetById(Guid id)
    {
        return await _dbcontext.Courses
            .Include(c => c.Tags)
            .Include(c => c.Platform)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public IQueryable<Course> GetAll()
    {
        return _dbcontext.Courses
            .Include(c => c.Tags)
            .Include(c => c.Platform)
            .AsNoTracking();
    }
    public void Update(Course course)
    {
        _dbcontext.Courses.Update(course);
    }
}
