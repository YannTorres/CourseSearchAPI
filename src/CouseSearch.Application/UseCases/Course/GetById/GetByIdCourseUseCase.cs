using CourseSearch.Communication.Extension;
using CourseSearch.Communication.Responses.Courses;
using CourseSearch.Domain.Repositories.Course;
using CourseSearch.Exception.ExceptionBase;

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

        List<Domain.Enums.CourseLevel>? domainCourseLevels = course?.CourseLevels;
        List<Communication.Enums.CourseLevel>? communicationCourseLevels = null;

        if (domainCourseLevels != null)
        {
            communicationCourseLevels = [.. domainCourseLevels.Select(domainEnum => (Communication.Enums.CourseLevel)domainEnum)];
        }

        return new ResponseCourseJson
        {
            Id = course?.Id ?? Guid.Empty,
            Title = course?.Title ?? string.Empty,
            Description = course?.Description ?? "Não Informada",
            Author = course?.Author ?? "Não Informado",
            CourseUrl = course?.CourseUrl ?? string.Empty,
            ImageUrl = course?.IconUrl ?? string.Empty,
            CourseLevels = communicationCourseLevels ?? [],
            DurationInMinutes = course?.DurationsInMinutes ?? 0,
            Locale = course?.Locale ?? string.Empty,
            Units = course?.Units ?? [],
            Popularity = course?.Popularity,
            Tags = course?.Tags?.Select(tag => new Tag
            {
                Id = tag.Id,
                Name = tag.Name
            }).ToList() ?? [],
            UpdatedAt = course?.UpdatedAt ?? DateTime.MinValue,
        };
    }
}
