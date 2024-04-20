﻿using BSMS.API.Extensions;
using BSMS.API.Filters;
using BSMS.Application.Features.Common;
using BSMS.Application.Features.Company.Commands.Create;
using BSMS.Application.Features.Company.Commands.Delete;
using BSMS.Application.Features.Company.Queries.GetAllShortInfo;
using BSMS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
[Authorization(Role.Admin)]
public class CompanyController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Create new transport company
    /// </summary>
    /// <param name="command">Company's data</param>
    /// <returns>ID of the created company</returns>
    [HttpPost("Create")]
    public async Task<ActionResult<CreatedEntityResponse>> Create(CreateCompanyCommand command)
    {
        var result = await sender.Send(command);

        return result.DecideWhatToReturn();
    }
    
    /// <summary>
    /// Delete specified company
    /// </summary>
    /// <param name="id">ID of the company</param>
    /// <returns>Message of action result</returns>
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult<MessageResponse>> Delete(int id)
    {
        var result = await sender.Send(new DeleteCompanyCommand(id));

        return result.DecideWhatToReturn();
    }

    /// <summary>
    /// Get companies' list
    /// </summary>
    /// <returns>List of companies with their IDs and names</returns>
    [HttpGet("GetAllShortInfo")]
    public async Task<ActionResult<List<GetAllCompaniesShortInfoResponse>>> GetAll()
    {
        return await sender.Send(new GetAllCompaniesShortInfoQuery());
    }
}