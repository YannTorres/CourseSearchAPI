
using CourseSearch.Communication.Responses.Courses;
using CourseSearch.Domain.Repositories.Course;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CourseSearch.Application.UseCases.Course.GetAll;
public class GetAllCoursesUseCase : IGetAllCoursesUseCase
{
    private readonly ICourseReadOnlyRepository _courseRepository;
    public GetAllCoursesUseCase(ICourseReadOnlyRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }
    public async Task<ResponseCoursesJson> Execute(int pageNumber, int pageSize)
    {
        var result = _courseRepository.GetAll();

        var totalCount = await result.CountAsync();

        var courses = await result
            .OrderBy(c => c.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var coursesMap = courses.Select(c => new ResponseShortCourseJson
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Platform = c.Platform.Name
        }).ToList();

        return new ResponseCoursesJson
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            Courses = coursesMap,
        };
    }
}
