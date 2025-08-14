using CourseSearch.Domain.Dtos.AluraCourses;
using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Services.CourseProvider;
using CourseSearch.Domain.Services.CourseProvider.HttpClient;
using Microsoft.IdentityModel.Tokens;

namespace CourseSearch.Infrastructure.Services.CoursesProvider;
public class AluraCourseProvider : ICourseProvider
{
    private readonly IAluraCourseApiClient _apiClient;
    private const int PlataformId = 3;

    public AluraCourseProvider(IAluraCourseApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    public string PlatformName => "Alura";

    public bool RequiresKeywordFiltering => false;

    public async IAsyncEnumerable<Course> FetchAllCoursesAsync(CancellationToken cancellationToken)
    {
        var courseList = await _apiClient.GetCourseListAsync();

        foreach (var courseSummary in courseList)
        {
            if (cancellationToken.IsCancellationRequested) break;

            var courseDetails = await _apiClient.GetCourseDetailsAsync(courseSummary.Slug);

            if (courseDetails != null && courseDetails.Showable)
            {

                yield return MapAluraDtoToDomain(courseDetails, courseSummary.Slug);
            }

            await Task.Delay(200, cancellationToken);
        }
    }

    private Course MapAluraDtoToDomain(AluraCourseDetailsDto dto, string slug)
    {
        var course = new Course
        {
            ExternalId = dto.Id.ToString(),
            Title = dto.Nome,
            Description = dto.Metadescription == null ? "" : dto.Metadescription!,
            CourseUrl = $"https://www.alura.com.br/curso-online-{slug}",
            Author = "Alura",
            DurationsInMinutes = dto.Duration > 0 ? dto.Duration : null,
            Locale = "pt-br",
            UpdatedAt = dto.DataAtualizacao,
            PlatformId = PlataformId,
        };

        var tags = new List<Tag>();
        if (dto.Categorias != null)
        {
            tags.AddRange(dto.Categorias.Select(c => new Tag { Name = c.Nome }));
        }
        if (dto.SubCategorias != null)
        {
            tags.AddRange(dto.SubCategorias.Select(sc => new Tag { Name = sc.Nome }));
        }
        course.Tags = tags;
        course.Rating = new Rating
        {
            CourseId = course.Id,
            Count = dto.QuantidadeAvaliacoes,
            Average = dto.Nota
        };

        return course;
    }
}
