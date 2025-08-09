using CourseSearch.Application.UseCases.Course.GetAll;
using CourseSearch.Application.UseCases.Course.GetById;
using CourseSearch.Communication.Responses.Courses;
using CourseSearch.Communication.Responses.Error;
using Microsoft.AspNetCore.Mvc;

namespace CourseSearch.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseCoursesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllCourses(
        [FromServices] IGetAllCoursesUseCase useCase,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize)
    {
        var response = await useCase.Execute(pageNumber, pageSize);

        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseCourseJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseById(
        [FromServices] IGetByIdCourseUseCase useCase,
        [FromRoute] Guid id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }
}
