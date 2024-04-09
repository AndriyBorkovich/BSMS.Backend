using BSMS.API.Extensions;
using BSMS.Application.Features.Common;
using BSMS.Application.Features.Passenger.Commands.Create;
using BSMS.Application.Features.Passenger.Commands.Delete;
using BSMS.Application.Features.Passenger.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
public class PassengerController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Create new passenger
    /// </summary>
    /// <param name="command">Passenger's data</param>
    /// <returns>ID of the created passenger</returns>
    [HttpPost("Create")]
    public async Task<ActionResult<CreatedEntityResponse>> Create(CreatePassengerCommand command)
    {
        var result = await sender.Send(command);

        return result.DecideWhatToReturn();
    }
    
    /// <summary>
    /// Delete specified passenger
    /// </summary>
    /// <param name="id">ID of the passenger</param>
    /// <returns>Message of action result</returns>
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult<MessageResponse>> Delete(int id)
    {
        var result = await sender.Send(new DeletePassengerCommand(id));

        return result.DecideWhatToReturn();
    }

    /// <summary>
    /// Get all passengers
    /// </summary>
    /// <param name="query">Filtering fields</param>
    /// <returns>List with passengers' data: full name, phone, email</returns>
    [HttpGet("GetAll")]
    public async Task<ActionResult<List<GetAllPassengersResponse>>> GetAll(
        [FromQuery] GetAllPassengersQuery query)
    {
        var result = await sender.Send(query);

        return result.DecideWhatToReturn();
    }
}