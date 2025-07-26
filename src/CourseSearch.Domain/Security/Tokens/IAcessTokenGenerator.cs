using CourseSearch.Domain.Entities;

namespace CourseSearch.Domain.Security.Tokens;
public interface IAcessTokenGenerator
{
    public string Generate(User user);
}
