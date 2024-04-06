using BSMS.API.Extensions;
using BSMS.Application.Features.Passenger.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
public class PassengerController(ISender mediator) : ControllerBase
{
    /// <summary>
    /// Create new passenger
    /// </summary>
    /// <param name="command">Passenger's data</param>
    /// <returns>ID of the created passenger</returns>
    [HttpPost("Create")]
    public async Task<ActionResult<int>> Create(CreatePassengerCommand command)
    {
        var result = await mediator.Send(command);

        return result.DecideWhatToReturn();
    }
}