using BSMS.API.Extensions;
using BSMS.Application.Features.Bus.Commands.Create;
using BSMS.Application.Features.Bus.Commands.Delete;
using BSMS.Application.Features.Bus.Queries.GetAll;
using BSMS.Application.Features.Common;
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
    public async Task<ActionResult<int>> Create(CreateBusCommand command)
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
    public async Task<ActionResult<MessageResponse>> Delete(int id)
    {
        var result = await sender.Send(new DeleteBusCommand(id));

        return result.DecideWhatToReturn();
    }

    /// <summary>
    /// Get all buses
    /// </summary>
    /// <param name="query">Filtering fields</param>
    /// <returns>List with bus data, driver and company name</returns>
    [HttpGet("GetAll")]
    public async Task<ActionResult<List<GetAllBusesResponse>>> GetAll(
        [FromQuery] GetAllBusesQuery query)
    {
        var result = await sender.Send(query);

        return result.DecideWhatToReturn();
    }
}