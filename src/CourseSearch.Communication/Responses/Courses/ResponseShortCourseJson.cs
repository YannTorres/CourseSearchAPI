using CourseSearch.Communication.Enums;

namespace CourseSearch.Communication.Responses.Courses;
public class ResponseShortCourseJson
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public List<string>? CourseLevels { get; set; }
    public List<string>? Tags { get; set; }
    public string? RatingAverage { get; set; }
    public string? RatingCount { get; set; }
}
