using BSMS.API.Extensions;
using BSMS.Infrastructure.Authorization.Models;
using BSMS.Infrastructure.Authorization.Services;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Controllers;

/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IAuthenticationService authenticationService) : ControllerBase
{
    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="request">New user data (username, email, password, role)</param>
    /// <returns>Username and role-based JWT token</returns>
    [HttpPost("Register")]
    public async Task<ActionResult<AuthenticateResponse>> Register(RegisterRequest request)
    {
        var result = await authenticationService.Register(request);

        return result.DecideWhatToReturn();
    }
    
    /// <summary>
    /// Login with credentials
    /// </summary>
    /// <param name="request">Credentials (username and password)</param>
    /// <returns>Username and role-based JWT token</returns>
    [HttpPost("Login")]
    public async Task<ActionResult<AuthenticateResponse>> Login(LoginRequest request)
    {
        var result = await authenticationService.Login(request);

        return result.DecideWhatToReturn();
    }
}