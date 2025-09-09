using CourseSearch.Communication.Responses.Courses;
using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Extensions;
using CourseSearch.Domain.Repositories.Course;
using Microsoft.EntityFrameworkCore;

namespace CourseSearch.Application.UseCases.Course.GetAll;
public class GetAllCoursesUseCase : IGetAllCoursesUseCase
{
    private readonly ICourseReadOnlyRepository _courseRepository;
    public GetAllCoursesUseCase(ICourseReadOnlyRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }
    public async Task<ResponseCoursesJson> Execute(int pageNumber, int pageSize, string? search, string? sortBy, string? sortOrder)
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

        var isDescending = sortOrder?.ToLower() == "desc";

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            switch (sortBy?.ToLower())
            {
                case "title":
                    result = isDescending
                        ? result.OrderByDescending(c => c.Title).ThenByDescending(c => c.Id)
                        : result.OrderBy(c => c.Title).ThenBy(c => c.Id);
                    break;
                case "rating":
                    result = isDescending
                        ? result.OrderByDescending(c => c.Rating == null ? 0 : c.Rating.Average).ThenByDescending(c => c.Id)
                        : result.OrderBy(c => c.Rating == null ? 0 : c.Rating.Average).ThenBy(c => c.Id);
                    break;
                case "ratingCount":
                    result = isDescending
                        ? result.OrderByDescending(c => c.Rating == null ? 0 : c.Rating.Count).ThenByDescending(c => c.Id)
                        : result.OrderBy(c => c.Rating == null ? 0 : c.Rating.Count).ThenBy(c => c.Id);
                    break;
                default:
                    result = result.OrderBy(c => c.Id);
                    break;
            }

        }

        var totalCount = await result.CountAsync();

        var courses = await result
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var coursesMap = courses.Select(c => new ResponseShortCourseJson
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Platform = c?.Author ?? "Autor não Definido",
            RatingAverage = c?.Rating?.Average.ToString("F1") ?? "N/A",
            RatingCount = (c?.Rating?.Count ?? 0).ToAvaliacoesString(),
            CourseLevels = c?.CourseLevels?.Select(cl => cl.CourseLevelToString()).ToList() ?? ["Nível Não Especificado"],
            Tags = c?.Tags.Select(t => t.Name).ToList() ?? ["Sem Tags"],
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
 