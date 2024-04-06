using BSMS.API.Extensions;
using BSMS.Application.Features.Bus.Commands;
using BSMS.Application.Features.Bus.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[Route("/api/[controller]")]
[ApiController]
public class BusController(ISender mediator) : ControllerBase
{
    /// <summary>
    /// Create new bus with it's schedule
    /// </summary>
    /// <param name="command">Bus data and its schedule data</param>
    /// <returns>ID of the created bus</returns>
    [HttpPost("Create")]
    public async Task<ActionResult<int>> Create(CreateBusCommand command)
    {
        var result = await mediator.Send(command);
        
        return result.DecideWhatToReturn();
    }
}