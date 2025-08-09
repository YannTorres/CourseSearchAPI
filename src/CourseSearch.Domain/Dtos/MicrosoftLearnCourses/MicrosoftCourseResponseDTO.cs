using CourseSearch.Domain.Dtos.EdxCourses;
using System.Text.Json.Serialization;

namespace CourseSearch.Domain.Dtos.MicrosoftLearningCourses;
public record MicrosoftCourseResponseDTO(
    [property: JsonPropertyName("modules")] IEnumerable<MicrosoftCourseDto> Courses
);

public record MicrosoftCourseDto(
    [property: JsonPropertyName("uid")] string Id,
    [property: JsonPropertyName("course_number")] string CourseNumber,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("summary")] string Summary,
    [property: JsonPropertyName("duration_in_minutes")] int? Duration,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("icon_url")] string IconUrl,
    [property: JsonPropertyName("locale")] string Locale,
    [property: JsonPropertyName("last_modified")] DateTime LastDateModified,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("levels")] IEnumerable<string> Levels,
    [property: JsonPropertyName("rating")] MicrosoftRatingDto? Rating,
    [property: JsonPropertyName("popularity")] float? Popularity,
    [property: JsonPropertyName("roles")] IEnumerable<string> Tags,
    [property: JsonPropertyName("products")] IEnumerable<string> MoreTags,
    [property: JsonPropertyName("units")] IEnumerable<string>? Units,
    [property: JsonPropertyName("number_of_children")] int? NumberOfChildren
);

public record MicrosoftRatingDto(
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("average")] float Average
);
