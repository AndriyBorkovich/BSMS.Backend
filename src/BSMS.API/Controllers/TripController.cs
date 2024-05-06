using BSMS.API.Extensions;
using BSMS.API.Filters;
using BSMS.Application.Features.Common;
using BSMS.Application.Features.Trip.Queries.GetAll;
using BSMS.Application.Features.Trip.Queries.GetAllStops;
using BSMS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc/>
[ApiController]
[Route("/api/[controller]")]
[Authorization(Role.Admin, Role.Driver, Role.Passenger)]
public class TripController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Get all available trips based on current moment
    /// </summary>
    /// <param name="query">Contains route search field and pagination data</param>
    /// <returns></returns>
    [HttpGet("GetAll")]
    public async Task<ActionResult<ListResponse<GetAllTripsQueryRespone>>> GetAll(
        [FromQuery] GetAllTripsQuery query
    )
    {
        return await sender.Send(query);
    }

    /// <summary>
    /// Get all route stops related to trip
    /// </summary>
    /// <param name="query">Contains trip ID</param>
    /// <returns>List of stop IDs and names</returns>
    [HttpGet("GetAllStops")]
    public async Task<ActionResult<List<GetAllRouteStopsResponse>>> GetAllStops(
        [FromQuery] GetAllRouteStopsQuery query
    )
    {
        var result = await sender.Send(query);

        return result.DecideWhatToReturn();
    }
}
