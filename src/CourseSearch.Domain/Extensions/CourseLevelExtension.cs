using CourseSearch.Domain.Enums;

namespace CourseSearch.Domain.Extensions;
public static class CourseLevelExtension
{
    public static string CourseLevelToString(this CourseLevel courseLevel)
    {
        return courseLevel switch
        {
            CourseLevel.Beginner => "Iniciante",
            CourseLevel.Intermediate => "Intermediário",
            CourseLevel.Advanced => "Avançado",
            CourseLevel.NotSpecified => "Nível não expecificado",
            _ => throw new ArgumentOutOfRangeException(nameof(courseLevel), courseLevel, null)
        };
    }
}
