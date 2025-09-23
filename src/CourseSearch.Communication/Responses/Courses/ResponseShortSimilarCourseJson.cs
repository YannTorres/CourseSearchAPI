namespace CourseSearch.Communication.Responses.Courses;
public class ResponseShortSimilarCourseJson
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public string? RatingAverage { get; set; }
    public string? RatingCount { get; set; }

}
