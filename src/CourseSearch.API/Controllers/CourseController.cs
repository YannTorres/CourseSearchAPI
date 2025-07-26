using CourseSearch.Application.UseCases.Course.GetAll;
using CourseSearch.Communication.Responses.Courses;
using Microsoft.AspNetCore.Mvc;

namespace CourseSearch.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseCoursesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllCourses(
        [FromServices] IGetAllCoursesUseCase useCase,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize)
    {
        var response = await useCase.Execute(pageNumber, pageSize);

        return Ok(response);
    }
}
