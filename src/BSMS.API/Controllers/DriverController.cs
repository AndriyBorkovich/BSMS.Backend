using BSMS.API.Extensions;
using BSMS.Application.Features.Driver.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
public class DriverController(ISender mediator) : ControllerBase
{
    /// <summary>
    /// Create new driver (with company and bus (both optional))
    /// </summary>
    /// <param name="command">Driver data</param>
    /// <returns>ID of the created driver</returns>
    [HttpPost("Create")]
    public async Task<ActionResult<int>> Create(CreateDriverCommand command)
    {
        var result = await mediator.Send(command);

        return result.DecideWhatToReturn();
    }
}