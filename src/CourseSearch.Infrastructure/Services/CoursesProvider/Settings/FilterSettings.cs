namespace CourseSearch.Infrastructure.Services.CoursesProvider.Settings;
public class FilterSettings
{
    public List<string> TechKeywords { get; set; } = new();
    public List<string> ExclusionKeywords { get; set; } = new();
}
