using BSMS.API.Filters;
using BSMS.Application.Features.Bus.Queries.GetTopByTravelledDistance;
using BSMS.Application.Features.Company.Queries.GetTopByRevenue;
using BSMS.Application.Features.Route.GetTopByBusRating;
using BSMS.Application.Features.Route.GetTopByRevenue;
using BSMS.Application.Features.Ticket.Queries.GetTicketDistibutionByPaymentType;
using BSMS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;
/// <inheritdoc/>
[ApiController]
[Route("/api/[controller]")]
[Authorization(Role.Admin)]
public class StatisticController(ISender sender): ControllerBase
{
    /// <summary>
    /// Get tickets distibution by payment type
    /// </summary>
    /// <returns>List of types and their counts</returns>
    [HttpGet("GetTicketDistributionByType")]
    public async Task<ActionResult<List<GetTicketDistibutionByPaymentTypeResponse>>> GetTicketDistibution()
    {
        return await sender.Send(new GetTicketDistibutionByPaymentTypeQuery());
    }

    /// <summary>
    /// Get top 10 transport companies by revenue
    /// </summary>
    /// <returns>List of companies and their total revenue (price of total sold tickets bought on company's buses trips)</returns>
    [HttpGet("GetTopCompaniesByRevenue")]
    public async Task<ActionResult<List<GetTopCompaniesByRevenueResponse>>> GetTopCompaniesByRevenue()
    {
        return await sender.Send(new GetTopCompaniesByRevenueQuery());
    }

    [HttpGet("GetTopRoutesByBusRating")]
    public async Task<ActionResult<List<GetTopRoutesByBusRatingResponse>>> GetTopRoutesByBusRating()
    {
        return await sender.Send(new GetTopRoutesByBusRatingQuery());
    }

    [HttpGet("GetTopRoutesByRevenue")]
    public async Task<ActionResult<List<GetTopRoutesByRevenueResponse>>> GetTopRoutesByRevenue()
    {
        return await sender.Send(new GetTopRoutesByRevenueQuery());
    }

    [HttpGet("GetTopBusesByTravelledDistance")]
    public async Task<ActionResult<List<GetTopBusesByTravelledDistanceResponse>>> GetTopBusesByTravelledDistance()
    {
        return await sender.Send(new GetTopBusesByTravelledDistanceQuery());
    }
}