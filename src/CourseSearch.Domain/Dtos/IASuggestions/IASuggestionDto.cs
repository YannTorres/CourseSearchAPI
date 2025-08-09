using System.Text.Json.Serialization;

namespace CourseSearch.Domain.Dtos.IASuggestions;
public class AISuggestionDto
{
    public List<AISuggestionContentDto> Roadmap { get; set; } = [];
}

public class AISuggestionContentDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
}