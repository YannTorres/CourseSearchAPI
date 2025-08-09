using CourseSearch.Domain.Dtos.EdxCourses;
using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Services.CourseProvider;
using CourseSearch.Domain.Services.CourseProvider.HttpClient;

namespace CourseSearch.Infrastructure.Services.EdxCourses;
public class EdxCourseProvider : ICourseProvider
{
    private readonly IEdxApiClient _edxApiClient;
    private const int PlataformId = 1;
    private const int PageSize = 20;

    public string PlatformName => "edX";
    public bool RequiresKeywordFiltering => true;

    public EdxCourseProvider(IEdxApiClient edxApiClient)
    {
        _edxApiClient = edxApiClient;
    }

    public async IAsyncEnumerable<Course> FetchAllCoursesAsync(CancellationToken cancellationToken)
    {
        var currentPage = 1;
        var hasNextPage = true;

        while (hasNextPage && !cancellationToken.IsCancellationRequested)
        {
            var response = await _edxApiClient.GetCoursesByPageAsync(currentPage, PageSize);

            if (response?.Results == null || !response.Results.Any())
            {
                hasNextPage = false;
                continue;
            }

            foreach (var dto in response.Results)
            {
                yield return MapEdxDtoToDomain(dto);
            }

            hasNextPage = !string.IsNullOrEmpty(response.Pagination.Next);
            currentPage++;
            await Task.Delay(1000, cancellationToken);
        }
    }

    private Course MapEdxDtoToDomain(EdxCourseDto dto)
    {
        return new Course
        {
            Id = Guid.NewGuid(),
            ExternalId = dto.CourseId,
            Title = dto.Name ?? "Título indisponível",
            Description = "Não Informada",
            Author = dto.Org,
            CourseUrl = $"https://courses.edx.org/courses/{dto.CourseId}/about",
            UpdatedAt = DateTime.UtcNow,
            PlatformId = PlataformId,
        };
    }
}
