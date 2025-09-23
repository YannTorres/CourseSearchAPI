namespace CourseSearch.Domain.Repositories.Rating;
public interface IRatingReadOnlyRepository
{
    public Task<List<Entities.UserCourseRating>?> GetAllRatingsByCourseId(Guid courseId);
}
