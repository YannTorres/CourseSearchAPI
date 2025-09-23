
using CourseSearch.Communication.Responses.Courses;
using CourseSearch.Communication.Responses.Rating;
using CourseSearch.Communication.Responses.Roadmap;
using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories.Course;
using CourseSearch.Domain.Repositories.Rating;
using CourseSearch.Domain.Services.LoggedUser;

namespace CourseSearch.Application.UseCases.Rating.GetRating;
public class GetCourseRatingUseCase : IGetCourseRatingUseCase
{
    private readonly IRatingReadOnlyRepository _readOnlyRepository;
    public GetCourseRatingUseCase(
        IRatingReadOnlyRepository readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
    }
    public async Task<List<ResponseGetCourseRatingJson>> Execute(Guid courseId)
    {
        var rating = await _readOnlyRepository.GetAllRatingsByCourseId(courseId);

        if (rating == null || rating.Count == 0) return [];

        return rating.Select(r => new ResponseGetCourseRatingJson
        {
            Rating = r.Score,
            Review = r.Review ?? string.Empty,
            UserName = r.User.FirstName + " " + r.User.LastName,
            UpdateAt = r.UpdateAt,
        }).ToList();
    }
}
