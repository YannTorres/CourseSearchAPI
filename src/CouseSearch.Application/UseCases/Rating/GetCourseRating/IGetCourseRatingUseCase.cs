using CourseSearch.Communication.Responses.Rating;

namespace CourseSearch.Application.UseCases.Rating.GetRating;
public interface IGetCourseRatingUseCase
{
    Task<List<ResponseGetCourseRatingJson>> Execute(Guid courseId);
}
