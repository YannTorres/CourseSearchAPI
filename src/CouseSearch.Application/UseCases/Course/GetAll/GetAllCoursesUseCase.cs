using CourseSearch.Communication.Responses.Courses;
using CourseSearch.Domain.Repositories.Course;
using CourseSearch.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CourseSearch.Application.UseCases.Course.GetAll;
public class GetAllCoursesUseCase : IGetAllCoursesUseCase
{
    private readonly ICourseReadOnlyRepository _courseRepository;
    public GetAllCoursesUseCase(ICourseReadOnlyRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }
    public async Task<ResponseCoursesJson> Execute(int pageNumber, int pageSize, string? search)
    {
        var result = _courseRepository.GetAll();

        if (!string.IsNullOrEmpty(search))
        {
            var searchTerm = search.Trim().ToLower();
            result = result.Where(a =>
                (a.Title != null && a.Title.ToLower().Contains(searchTerm)) ||
                (a.Description != null && a.Description.ToLower().Contains(searchTerm))
            );
        }
        
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
            Platform = c.Platform.Name,
            RatingAverage = c.Rating?.Average.ToString() ?? "Não Definido" ,
            RatingCount = c.Rating?.Count.ToString() ?? "Não Definido",
            CourseLevels = c.CourseLevels?.Select(cl => cl.CourseLevelToString()).ToList() ?? ["Nível Não Especificado"],
            Tags = c.Tags.Select(t => t.Name).ToList()
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
 