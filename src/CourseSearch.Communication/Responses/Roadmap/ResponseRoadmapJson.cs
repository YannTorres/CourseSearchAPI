namespace CourseSearch.Communication.Responses.Roadmap;
public class ResponseRoadmapJson
{
    public Guid Id { get; set; }
    public string RoadmapTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RoadmapLevel { get; set; } = string.Empty;
    public int Steps { get; set; }
    public List<ResponseRoadmapCourseJson> Courses { get; set; } = [];
}
