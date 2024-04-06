using BSMS.API.Extensions;
using BSMS.Application.Features.Company.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
public class CompanyController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Create new transport company
    /// </summary>
    /// <param name="command">Company's data</param>
    /// <returns>ID of the created company</returns>
    [HttpPost("Create")]
    public async Task<ActionResult<int>> Create(CreateCompanyCommand command)
    {
        var result = await sender.Send(command);

        return result.DecideWhatToReturn();
    }
}