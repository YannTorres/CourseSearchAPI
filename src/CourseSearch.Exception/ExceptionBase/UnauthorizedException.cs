namespace CourseSearch.Exception.ExceptionBase;
public class UnauthorizedException : CourseSearchException
{
    public UnauthorizedException() : base("Email ou senha incorretos.") { }
    public override int StatusCode => 401;
    public override List<string> GetErrors() => [Message];
}
