namespace CourseSearch.Domain.Security.Tokens;
public interface ITokenProvider
{
    string TokenOnRequest();
}
