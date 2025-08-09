using CourseSearch.Communication.Enums;
using System.ComponentModel.DataAnnotations;

namespace CourseSearch.Communication.Requests.Roadmap;
public class RequestGenerateRoadpmapJson
{
    public string Objective { get; set; } = string.Empty;
    public string AreaOfInterest { get; set; } = string.Empty;
    public CourseLevel ExperienceLevel { get; set; }
}
