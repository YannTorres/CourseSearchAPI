namespace CourseSearch.Communication.Responses.Rating;
public class ResponseGetCourseRatingJson
{
    public int Rating { get; set; }
    public string Review { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime UpdateAt { get; set; }
}
