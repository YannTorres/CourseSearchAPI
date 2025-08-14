using CourseSearch.Communication.Enums;
using CourseSearch.Communication.Extension;

namespace CourseSearch.Communication.Responses.Courses;
public class ResponseCourseJson
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CourseUrl { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int? DurationInMinutes { get; set; }
    public List<string>? CourseLevels { get; set; }
    public List<string>? Tags { get; set; }
    public string? RatingAverage { get; set; }
    public string? RatingCount { get; set; }
    public DateTime UpdatedAt { get; set; }
}
