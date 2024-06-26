﻿using BSMS.API.Extensions;
using BSMS.API.Filters;
using BSMS.Application.Features.Common;
using BSMS.Application.Features.Route.Commands.Create;
using BSMS.Application.Features.Route.Commands.Delete;
using BSMS.Application.Features.Route.Queries.GetAll;
using BSMS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
[Authorization(Role.Admin)]
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

    /// <summary>
    /// Get all available routes
    /// </summary>
    /// <returns>List with route ID, origin and destination names</returns>
    [HttpGet("GetAll")]
    public async Task<ActionResult<List<GetAllRoutesResponse>>> GetAll()
    {
        return await sender.Send(new GetAllRoutesQuery());
    }
}