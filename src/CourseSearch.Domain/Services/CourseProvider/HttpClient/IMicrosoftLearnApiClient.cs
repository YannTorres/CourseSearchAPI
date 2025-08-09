using CourseSearch.Domain.Dtos.MicrosoftLearningCourses;

namespace CourseSearch.Domain.Services.CourseProvider.HttpClient;
public interface IMicrosoftLearnApiClient
{
    Task<MicrosoftCourseResponseDTO?> GetFullCatalogAsync();
}
