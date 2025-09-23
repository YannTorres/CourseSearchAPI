using CourseSearch.Communication.Requests.Courses;

namespace CourseSearch.Application.UseCases.Rating.AddRating;
public interface IAddRatingUseCase
{
    public Task Execute(Guid courseId, RequestAddRatingJson rating);
}
