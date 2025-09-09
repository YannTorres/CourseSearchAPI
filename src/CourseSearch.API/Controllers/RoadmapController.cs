using CourseSearch.Application.UseCases.Roadmap.Create;
using CourseSearch.Application.UseCases.Roadmap.GetAll;
using CourseSearch.Application.UseCases.Roadmap.GetById;
using CourseSearch.Communication.Requests.Roadmap;
using CourseSearch.Communication.Responses.Error;
using CourseSearch.Communication.Responses.Roadmap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseSearch.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RoadmapController : ControllerBase
{
    [HttpPost("generate-with-ai")]
    [ProducesResponseType(typeof(ResponseGenerateRoadmapJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GenerateRoadmapWithAI(
        [FromBody] RequestGenerateRoadpmapJson request,
        [FromServices] ICreateRoadmapUseCase useCase)
    {
        var response = await useCase.Execute(request);
        
        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseRoadmapsJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll(
        [FromServices] IGetAllRoadmapUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Roadmaps.Count == 0)
            return NoContent();

        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseRoadmapJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById(
        [FromServices] IGetByIdRoadmapUseCase useCase,
        [FromRoute] Guid id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }
}
