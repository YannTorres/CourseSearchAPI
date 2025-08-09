using CourseSearch.Domain.Entities;

namespace CourseSearch.Domain.Services.CourseProvider;
public interface ICourseProvider
{
    string PlatformName { get; }
    bool RequiresKeywordFiltering { get; }
    IAsyncEnumerable<Course> FetchAllCoursesAsync(CancellationToken cancellationToken);
}
