using BSMS.API.Extensions;
using BSMS.API.Filters;
using BSMS.Application.Features.Common;
using BSMS.Application.Features.Driver.Commands.Create;
using BSMS.Application.Features.Driver.Commands.Delete;
using BSMS.Application.Features.Driver.Queries.GetAllFromCompany;
using BSMS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
[Authorization(Role.Admin)]
public class DriverController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Create new driver (with company and bus (both optional))
    /// </summary>
    /// <param name="command">Driver data</param>
    /// <returns>ID of the created driver</returns>
    [HttpPost("Create")]
    public async Task<ActionResult<CreatedEntityResponse>> Create(CreateDriverCommand command)
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

    /// <summary>
    /// Get all drivers from specific company
    /// </summary>
    /// <param name="companyId">ID of company</param>
    /// <returns>List of drivers IDs and names</returns>
    [HttpGet("GetAllFromCompany")]
    public async Task<ActionResult<List<GetAllDriversFromCompanyResponse>>> GetAllFromCompany(int companyId)
    {
        var result = await sender.Send(new GetAllDriversFromCompanyQuery(companyId));

        return result.DecideWhatToReturn();
    }
}