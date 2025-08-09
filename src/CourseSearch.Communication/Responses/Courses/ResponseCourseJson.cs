using CourseSearch.Communication.Enums;
using CourseSearch.Communication.Extension;

namespace CourseSearch.Communication.Responses.Courses;
public class ResponseCourseJson
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CourseUrl { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int DurationInMinutes { get; set; }
    public string Locale { get; set; } = string.Empty;
    public List<CourseLevel> CourseLevels { get; set; } = [];
    public List<string> Units { get; set; } = [];
    public float? Popularity { get; set; }
    public List<Tag> Tags { get; set; } = [];
    public DateTime UpdatedAt { get; set; }
}
