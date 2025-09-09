using CourseSearch.Communication.Extension;
using CourseSearch.Communication.Responses.Courses;
using CourseSearch.Domain.Repositories.Course;
using CourseSearch.Exception.ExceptionBase;
using CourseSearch.Domain.Extensions;

namespace CourseSearch.Application.UseCases.Course.GetById;
public class GetByIdCourseUseCase : IGetByIdCourseUseCase
{
    private readonly ICourseReadOnlyRepository _repository;
    public GetByIdCourseUseCase(ICourseReadOnlyRepository repository)
    {
        _repository = repository;
    }
    public async Task<ResponseCourseJson> Execute(Guid id)
    {
        var course = await _repository.GetById(id);

        if (course == null)
            throw new NotFoundException("Curso não encontrado");

        return new ResponseCourseJson
        {
            Id = course!.Id,
            Title = course!.Title,
            Description = course?.Description ?? "Não Informada",
            Platform = course?.Author ?? "Autor não definido",
            CourseUrl = course?.CourseUrl ?? string.Empty,
            ImageUrl = course?.IconUrl ?? string.Empty,
            CourseLevels = course?.CourseLevels?.Select(cl => cl.CourseLevelToString()).ToList() ?? ["Nível Não Especificado"],
            DurationInMinutes = course?.DurationsInMinutes ?? 0,
            Tags = course?.Tags.Select(t => t.Name).ToList(),
            UpdatedAt = course!.UpdatedAt,
            RatingAverage = course?.Rating?.Average.ToString("F1") ?? "N/A",
            RatingCount = (course?.Rating?.Count ?? 0).ToAvaliacoesString(),
        };
    }
}
