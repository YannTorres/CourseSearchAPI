using System.Text.Json.Serialization;

namespace CourseSearch.Domain.Dtos.EdxCourses;
public record EdxCourseResponseDto(
    [property: JsonPropertyName("pagination")] PaginationDto Pagination,
    [property: JsonPropertyName("results")] IEnumerable<EdxCourseDto> Results
);
public record EdxCourseDto(
    [property: JsonPropertyName("course_id")] string CourseId,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("short_description")] string? Description,
    [property: JsonPropertyName("org")] string? Org,
    [property: JsonPropertyName("number")] string? Number,
    [property: JsonPropertyName("start")] DateTime? Start,
    [property: JsonPropertyName("end")] DateTime? End,
    [property: JsonPropertyName("pacing")] string? Pacing
);

public record PaginationDto(
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("num_pages")] int NumPages,
    [property: JsonPropertyName("previous")] string? Previous,
    [property: JsonPropertyName("next")] string? Next
);