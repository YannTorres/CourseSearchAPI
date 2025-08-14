using CourseSearch.Domain.Dtos.MicrosoftLearningCourses;
using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Enums;
using CourseSearch.Domain.Services.CourseProvider;
using CourseSearch.Domain.Services.CourseProvider.HttpClient;

namespace CourseSearch.Infrastructure.Services.CoursesProvider;
public class MicrosoftLearnCourseProvider : ICourseProvider
{
    private readonly IMicrosoftLearnApiClient _apiClient;
    private const int PlataformId = 2;

    public MicrosoftLearnCourseProvider(IMicrosoftLearnApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public string PlatformName => "Microsoft Learn";
    public bool RequiresKeywordFiltering => false;

    public async IAsyncEnumerable<Course> FetchAllCoursesAsync(CancellationToken cancellationToken)
    {
        var catalog = await _apiClient.GetFullCatalogAsync();

        if (catalog?.Courses == null) yield break;

        foreach (var moduleDto in catalog.Courses)
        {
            if (cancellationToken.IsCancellationRequested) break;
            yield return MapMicrosoftDtoToDomain(moduleDto);
        }
    }

    private Course MapMicrosoftDtoToDomain(MicrosoftCourseDto dto)
    {

        ICollection<Tag> tags = [];
        if (dto.Tags != null)
        {
            foreach (var tag in dto.Tags)
            {
                tags.Add(new Tag
                {
                    Name = tag
                });
            }
        }

        if (dto.MoreTags != null)
        {
            foreach (var tag in dto.MoreTags)
            {
                tags.Add(new Tag
                {
                    Name = tag
                });
            }
        }

        var course = new Course
        {
            Id = Guid.NewGuid(),
            ExternalId = dto.Id,
            Title = dto.Title,
            Description = dto.Summary,
            CourseUrl = dto.Url,
            Author = "Microsoft Learn",
            IconUrl = dto.IconUrl,
            DurationsInMinutes = dto.Duration,
            Locale = dto.Locale,
            CourseLevels = MapLevelsFromDto(dto.Levels),
            Units = dto.Units?.ToList() ?? [],
            Popularity = dto.Popularity,
            UpdatedAt = dto.LastDateModified,
            PlatformId = PlataformId,
            Tags = tags
        };

        if (dto.Rating != null)
        {
            course.Rating = new Rating
            {
                Count = dto.Rating.Count,
                Average = dto.Rating.Average,
                CourseId = course.Id
            };
        }

        return course;
    }
    private List<CourseLevel> MapLevelsFromDto(IEnumerable<string>? levelStringsFromApi)
    {
        var mappedLevels = new List<CourseLevel>();

        if (levelStringsFromApi == null || !levelStringsFromApi.Any())
        {
            mappedLevels.Add(CourseLevel.NotSpecified);
            return mappedLevels;
        }

        foreach (var levelString in levelStringsFromApi)
        {
            if (Enum.TryParse<CourseLevel>(levelString, true, out var parsedLevel))
            {
                mappedLevels.Add(parsedLevel);
            }
        }
        return mappedLevels;
    }
}
