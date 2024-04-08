using BSMS.API.Extensions;
using BSMS.Application.Features.BusReview.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[Route("/api/[controller]")]
[ApiController]
public class BusReviewController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Create new passenger's review on specific bus
    /// </summary>
    /// <param name="command">Bus and passenger IDs, marks data</param>
    /// <returns>ID of the created review</returns>
    [HttpPost("Create")]
    public async Task<ActionResult<int>> Create(CreateBusReviewCommand command)
    {
        var result = await sender.Send(command);
        
        return result.DecideWhatToReturn();
    }
}