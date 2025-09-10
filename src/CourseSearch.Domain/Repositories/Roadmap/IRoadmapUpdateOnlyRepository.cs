namespace CourseSearch.Domain.Repositories.Roadmap;
public interface IRoadmapUpdateOnlyRepository
{
    Task<bool> RoadmapCourseExists(Entities.User user, Guid roadmapId, Guid courseId);
    Task UpdateStatus(Guid roadmapId, Guid CourseId, bool status);
}
