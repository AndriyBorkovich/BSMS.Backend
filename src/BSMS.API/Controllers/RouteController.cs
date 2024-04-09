using BSMS.API.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Features.Route.Commands.Create;
using BSMS.Application.Features.Route.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
public class RouteController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Create new route (with stops included)
    /// </summary>
    /// <param name="command">Contains origin and destination of route and list of it's stops names</param>
    /// <returns>ID of the created route</returns>
    [HttpPost("Create")]
    public async Task<ActionResult<CreatedEntityResponse>> Create(CreateRouteCommand command)
    {
        var result = await sender.Send(command);

        return result.DecideWhatToReturn();
    }
    
    /// <summary>
    /// Delete specified route
    /// </summary>
    /// <param name="id">ID of the route</param>
    /// <returns>Message of action result</returns>
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult<MessageResponse>> Delete(int id)
    {
        var result = await sender.Send(new DeleteRouteCommand(id));

        return result.DecideWhatToReturn();
    }
}