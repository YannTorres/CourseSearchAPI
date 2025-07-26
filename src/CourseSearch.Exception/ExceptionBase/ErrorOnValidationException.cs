using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CourseSearch.Exception.ExceptionBase;
public class ErrorOnValidationException : CourseSearchException
{
    public ErrorOnValidationException(List<string> message)
    {
        _errors = message;
    }

    private readonly List<string> _errors;
    public override int StatusCode => 400;
    public override List<string> GetErrors()
    {
        return _errors;
    }
}
