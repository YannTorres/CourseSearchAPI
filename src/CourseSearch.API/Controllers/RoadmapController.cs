using CourseSearch.Application.UseCases.Roadmap.Create;
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
}
