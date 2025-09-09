using CourseSearch.Communication.Enums;
using System.ComponentModel.DataAnnotations;

namespace CourseSearch.Communication.Requests.Roadmap;
public class RequestGenerateRoadpmapJson
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Objective { get; set; } = string.Empty;
    public string PriorityTechs { get; set; } = string.Empty;
    public CourseLevel ExperienceLevel { get; set; }
}
