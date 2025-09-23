using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories.Rating;
using Microsoft.EntityFrameworkCore;

namespace CourseSearch.Infrastructure.DataAcess.Repositories;
internal class RatingRepository : IRatingReadOnlyRepository
{
    private readonly CourseSearchDbContext _dbContext;
    public RatingRepository(CourseSearchDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserCourseRating>?> GetAllRatingsByCourseId(Guid courseId)
    {
        return await _dbContext.UserCourseRating
            .Include(r => r.User)
            .Where(r => r.CourseId == courseId)
            .ToListAsync();
    }
}
