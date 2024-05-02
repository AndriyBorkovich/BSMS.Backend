using BSMS.API.Filters;
using BSMS.Application.Features.Common;
using BSMS.Application.Features.Trip.Queries.GetAll;
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
}
