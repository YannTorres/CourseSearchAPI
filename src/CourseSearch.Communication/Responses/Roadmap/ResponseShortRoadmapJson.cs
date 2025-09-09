using CourseSearch.Communication.Enums;

namespace CourseSearch.Communication.Responses.Roadmap;
public class ResponseShortRoadmapJson
{
    public Guid Id { get; set; } = Guid.Empty;
    public CourseLevel ExperienceLevel { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int StepsCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
