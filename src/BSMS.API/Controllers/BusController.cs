using BSMS.API.Extensions;
using BSMS.API.Filters;
using BSMS.Application.Features.Bus.Commands.Create;
using BSMS.Application.Features.Bus.Commands.Delete;
using BSMS.Application.Features.Bus.Commands.Edit;
using BSMS.Application.Features.Bus.Queries.GetAll;
using BSMS.Application.Features.Common;
using BSMS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[Route("/api/[controller]")]
[ApiController]
public class BusController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Create new bus with it's schedule
    /// </summary>
    /// <param name="command">Bus data and its schedule data</param>
    /// <returns>ID of the created bus</returns>
    [HttpPost("Create")]
    [Authorization(Role.Admin)]
    public async Task<ActionResult<CreatedEntityResponse>> Create(CreateBusCommand command)
    {
        var result = await sender.Send(command);
        
        return result.DecideWhatToReturn();
    }

    /// <summary>
    /// Delete specified bus
    /// </summary>
    /// <param name="id">ID of the bus</param>
    /// <returns>Message of action result</returns>
    [HttpDelete("Delete/{id}")]
    [Authorization(Role.Admin)]
    public async Task<ActionResult<MessageResponse>> Delete(int id)
    {
        var result = await sender.Send(new DeleteBusCommand(id));

        return result.DecideWhatToReturn();
    }

    /// <summary>
    /// Edit existing bus data
    /// </summary>
    /// <param name="command">Contains edited bus ID, new brand, number, capacity values</param>
    /// <returns>Message of action result</returns>
    [HttpPost("Edit")]
    [Authorization(Role.Admin)]
    public async Task<ActionResult<MessageResponse>> Edit(EditBusCommand command)
    {
        var result = await sender.Send(command);

        return result.DecideWhatToReturn();
    }

    /// <summary>
    /// Get all buses
    /// </summary>
    /// <param name="query">Filtering fields and pagination params</param>
    /// <returns>List with bus data, driver and company name</returns>
    [HttpGet("GetAll")]
    [Authorization(Role.Admin, Role.Passenger)]
    public async Task<ActionResult<ListResponse<GetAllBusesResponse>>> GetAll(
        [FromQuery] GetAllBusesQuery query)
    {
        var result = await sender.Send(query);

        return result.DecideWhatToReturn();
    }
}