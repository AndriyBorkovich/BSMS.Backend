using BSMS.API.Extensions;
using BSMS.API.Filters;
using BSMS.Application.Features.Common;
using BSMS.Application.Features.Ticket.Commands.Create;
using BSMS.Application.Features.Ticket.Queries.GetAllPayments;
using BSMS.Application.Features.Ticket.Queries.GetPrice;
using BSMS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;
/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
public class TicketController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Create new ticket (with status)
    /// </summary>
    /// <param name="command">Contains stops, seat, price data</param>
    /// <returns></returns>
    [HttpPost("Create")]
    [Authorization(Role.Admin, Role.Passenger)]
    public async Task<ActionResult<CreatedEntityResponse>> Create(CreateTicketCommand command)
    {
        var result = await sender.Send(command);

        return result.DecideWhatToReturn();
    }
    
    /// <summary>
    /// Get all payments provided by passenger
    /// </summary>
    /// <param name="query">Contains only pagination data</param>
    /// <returns>List of payment date (who, how, when, where)</returns>
    [HttpGet("GetAllPayments")]
    [Authorization(Role.Admin)]
    public async Task<ActionResult<ListResponse<GetAllTicketPaymentsResponse>>> GetAllPayments(
        [FromQuery] GetAllTicketPaymentsQuery query)
    {
        return await sender.Send(query);
    }

    /// <summary>
    /// Calculate ticket price based on distance between stops
    /// </summary>
    /// <param name="query">Contains start and end stop IDs</param>
    /// <returns>Price of the ticket</returns>
    [HttpGet("GetPrice")]
    [Authorization(Role.Admin, Role.Passenger)]
    public async Task<ActionResult<GetTicketPriceResponse>> GetPrice(
        [FromQuery] GetTicketPriceQuery query) 
    {
        var result = await sender.Send(query);

        return result.DecideWhatToReturn();
    }
}