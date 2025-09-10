namespace CourseSearch.Communication.Responses.Roadmap;
public class ResponseRoadmapCourseJson
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int StepOrder { get; set; }
    public bool IsCompleted { get; set; }
    public int? DurationInMinutes { get; set; }
    public List<string>? CourseLevels { get; set; }
    public string? RatingAverage { get; set; }
    public string? PlatformName { get; set; }
}

