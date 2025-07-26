namespace CourseSearch.Exception.ExceptionBase;
public abstract class CourseSearchException : System.Exception
{
    protected CourseSearchException() { }
    protected CourseSearchException(string message) : base(message) { }
    public abstract int StatusCode { get; }
    public abstract List<string> GetErrors();
}
