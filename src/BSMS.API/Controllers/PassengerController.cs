﻿using BSMS.API.Extensions;
using BSMS.API.Filters;
using BSMS.Application.Features.Common;
using BSMS.Application.Features.Passenger.Commands.Create;
using BSMS.Application.Features.Passenger.Commands.Delete;
using BSMS.Application.Features.Passenger.Commands.Edit;
using BSMS.Application.Features.Passenger.Queries.GetAll;
using BSMS.Application.Features.Passenger.Queries.GetAllShortInfo;
using BSMS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
[Authorization(Role.Admin)]
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
    /// Edit existing passenger
    /// </summary>
    /// <param name="command">Passenger's new data</param>
    /// <returns>Message of action result</returns>
    [HttpPost("Edit")]
    public async Task<ActionResult<MessageResponse>> Edit(EditPassengerCommand command)
    {
        var result = await sender.Send(command);

        return result.DecideWhatToReturn();
    }

    /// <summary>
    /// Get all passengers
    /// </summary>
    /// <param name="query">Filtering fields and pagination params</param>
    /// <returns>List with passengers' data: full name, phone, email</returns>
    [HttpGet("GetAll")]
    public async Task<ActionResult<ListResponse<GetAllPassengersResponse>>> GetAll(
        [FromQuery] GetAllPassengersQuery query)
    {
        var result = await sender.Send(query);

        return result.DecideWhatToReturn();
    }

    /// <summary>
    /// Get passenger full names
    /// </summary>
    /// <param name="query">Contains optional bus query ID parameter which will help to get bus-related passengers</param>
    /// <returns>List containing passengers' IDs and fullnames</returns>
    [HttpGet("GetAllShortInfo")]
    public async Task<ActionResult<List<GetAllPassengersShortInfoResponse>>> GetAllShortInfo(
        [FromQuery] GetAllPassengersShortInfoQuery query) 
    {
        var result = await sender.Send(query);

        return result.DecideWhatToReturn();
    }
}