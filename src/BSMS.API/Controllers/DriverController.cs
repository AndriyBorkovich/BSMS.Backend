using BSMS.API.Extensions;
using BSMS.Application.Features.Bus.Commands.Delete;
using BSMS.Application.Features.Common;
using BSMS.Application.Features.Driver.Commands.Create;
using BSMS.Application.Features.Driver.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
public class DriverController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Create new driver (with company and bus (both optional))
    /// </summary>
    /// <param name="command">Driver data</param>
    /// <returns>ID of the created driver</returns>
    [HttpPost("Create")]
    public async Task<ActionResult<int>> Create(CreateDriverCommand command)
    {
        var result = await sender.Send(command);

        return result.DecideWhatToReturn();
    }
    
    /// <summary>
    /// Delete specified driver
    /// </summary>
    /// <param name="id">ID of the driver</param>
    /// <returns>Message of action result</returns>
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult<MessageResponse>> Delete(int id)
    {
        var result = await sender.Send(new DeleteDriverCommand(id));

        return result.DecideWhatToReturn();
    }
}