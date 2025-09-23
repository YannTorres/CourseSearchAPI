using CourseSearch.Application.UseCases.Rating.AddRating;
using CourseSearch.Application.UseCases.Rating.GetRating;
using CourseSearch.Communication.Requests.Courses;
using CourseSearch.Communication.Responses.Error;
using CourseSearch.Communication.Responses.Rating;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseSearch.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RatingController : ControllerBase
{
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddRating(
    [FromServices] IAddRatingUseCase useCase,
    [FromBody] RequestAddRatingJson rating,
    [FromRoute] Guid id)
    {
        await useCase.Execute(id, rating);

        return NoContent();
    }

    [HttpGet("{courseid}")]
    [ProducesResponseType(typeof(ResponseGetCourseRatingJson),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetCourseRating(
        [FromRoute] Guid courseid,
        [FromServices] IGetCourseRatingUseCase useCase)
    {
        var response = await useCase.Execute(courseid);

        if (response.Count == 0)
            return NoContent();

        return Ok(response);
    }

    [HttpGet("user/{userid}")]
    [ProducesResponseType(typeof(ResponseGetCourseRatingJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetUserRating(
    [FromRoute] Guid userid,
    [FromServices] IGetCourseRatingUseCase useCase)
    {
        var response = await useCase.Execute(userid);

        return Ok(response);
    }

}
