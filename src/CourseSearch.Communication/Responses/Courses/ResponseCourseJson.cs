using CourseSearch.Communication.Extension;

namespace CourseSearch.Communication.Responses.Courses;
public class ResponseCourseJson
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Tag> Tags { get; set; } = [];
    public string Instructor { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
}
