namespace CourseSearch.Communication.Responses.Courses;
public class ResponseShortCourseJson
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
}
