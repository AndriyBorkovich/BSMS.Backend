using BSMS.API.Extensions;
using BSMS.Application.Features.Route.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
public class RouteController(ISender mediator) : ControllerBase
{
    /// <summary>
    /// Create new route (with stops included)
    /// </summary>
    /// <param name="command">Contains origin and destination of route and list of it's stops names</param>
    /// <returns>ID of the created route</returns>
    [HttpPost("Create")]
    public async Task<ActionResult<int>> Create(CreateRouteCommand command)
    {
        var result = await mediator.Send(command);

        return result.DecideWhatToReturn();
    }
}