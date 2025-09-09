using CourseSearch.Communication.Requests.Courses;

namespace CourseSearch.Application.UseCases.Course.AddRating;
public interface IAddRatingUseCase
{
    public Task Execute(Guid courseId, RequestAddRatingJson rating);
}
