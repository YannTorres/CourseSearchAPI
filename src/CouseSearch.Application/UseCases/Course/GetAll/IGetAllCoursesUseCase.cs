using CourseSearch.Communication.Responses.Courses;

namespace CourseSearch.Application.UseCases.Course.GetAll;
public interface IGetAllCoursesUseCase
{
    Task<ResponseCoursesJson> Execute(int pageNumber, int pageSize, string? search);
}
