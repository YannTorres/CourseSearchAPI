using CourseSearch.Application.UseCases.Course.GetAll;
using CourseSearch.Application.UseCases.Course.GetById;
using CourseSearch.Application.UseCases.Course.Similar;
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
        [FromQuery] int pageSize,
        [FromQuery] string? sortby = null,
        [FromQuery] string? sortOrder = null,
        [FromQuery] string? search = null)
    {
        var response = await useCase.Execute(pageNumber, pageSize, search, sortby, sortOrder);

        if (response.Courses.Count == 0)
            return NoContent();

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

    [HttpGet("{id}/similar")]
    [ProducesResponseType(typeof(ResponseSimilarCoursesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSimilarCourses(
        [FromServices] IGetSimilarCoursesUseCase useCase,
        [FromRoute] Guid id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

}
