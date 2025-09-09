using CourseSearch.Communication.Responses.Courses;

namespace CourseSearch.Application.UseCases.Course.Similar;
public interface IGetSimilarCoursesUseCase
{
    Task<ResponseSimilarCoursesJson> Execute(Guid id);
}
