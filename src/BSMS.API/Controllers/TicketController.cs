using BSMS.API.Extensions;
using BSMS.API.Filters;
using BSMS.Application.Features.Common;
using BSMS.Application.Features.Ticket.Commands.Create;
using BSMS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;
/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
[Authorization(Role.Admin)]
public class TicketController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Create new ticket (with status)
    /// </summary>
    /// <param name="command">Contains stops, seat, price data</param>
    /// <returns></returns>
    [HttpPost("Create")]
    public async Task<ActionResult<CreatedEntityResponse>> Create(CreateTicketCommand command)
    {
        var result = await sender.Send(command);

        return result.DecideWhatToReturn();
    }
}