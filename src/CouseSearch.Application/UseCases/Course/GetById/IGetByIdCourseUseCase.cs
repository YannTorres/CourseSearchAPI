using CourseSearch.Communication.Responses.Courses;

namespace CourseSearch.Application.UseCases.Course.GetById;
public interface IGetByIdCourseUseCase
{
    Task<ResponseCourseJson> Execute(Guid id);
}
