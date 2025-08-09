using CourseSearch.Communication.Extension;

namespace CourseSearch.Communication.Responses.Users;
public class ResponseLoginUserJson
{
    public UserResponse User { get; set; } = new UserResponse();
    public string Token { get; set; } = string.Empty;
}
