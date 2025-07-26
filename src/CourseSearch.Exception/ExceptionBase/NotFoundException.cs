namespace CourseSearch.Exception.ExceptionBase;
public class NotFoundException : CourseSearchException
{
    public NotFoundException(string message) : base(message) { }
    public override int StatusCode => 404;
    public override List<string> GetErrors() => [Message];
}
