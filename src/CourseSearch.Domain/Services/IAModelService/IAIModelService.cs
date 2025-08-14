using CourseSearch.Communication.Requests.Roadmap;
using CourseSearch.Domain.Dtos.IASuggestions;
using CourseSearch.Domain.Entities;

namespace CourseSearch.Domain.Services.IAModelService;
public interface IAIModelService
{
    Task<List<string>> ExtractTopicsFromObjectiveAsync(RequestGenerateRoadpmapJson request);
    Task<List<AISuggestionContentDto>?> SelectAndOrderCoursesAsync(RequestGenerateRoadpmapJson request, IEnumerable<Course> candidateCourses);
}
