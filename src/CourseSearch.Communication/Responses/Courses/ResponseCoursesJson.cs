﻿namespace CourseSearch.Communication.Responses.Courses;
public class ResponseCoursesJson
{
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public List<ResponseShortCourseJson> Courses { get; set; } = [];
}
