using System.Text.Json.Serialization;

namespace CourseSearch.Domain.Dtos.MicrosoftLearningCourses;
public record MicrosoftCourseResponseDTO(
    [property: JsonPropertyName("uid")] int Id,
    [property: JsonPropertyName("course_number")] int CourseNumber,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("summary")] string Summary,
    [property: JsonPropertyName("duration_in_hours")] int Duration,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("locales")] List<string> Locales,
    [property: JsonPropertyName("last_modified")] DateTime LastDateModified,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("levels")] List<string> Levels,
    [property: JsonPropertyName("roles")] List<string> Tags,
    [property: JsonPropertyName("products")] string MoreTags,
    [property: JsonPropertyName("study_guide")] List<StudyGuide> StudyGuide,
    [property: JsonPropertyName("recommendation_list")] List<string> RecommendationList

);

public record StudyGuide(
    [property: JsonPropertyName("uid")] string Id,
    [property: JsonPropertyName("type")] string Type
);
