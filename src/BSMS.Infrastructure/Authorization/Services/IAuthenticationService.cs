using BSMS.Application.Helpers;
using BSMS.Infrastructure.Authorization.Models;

namespace BSMS.Infrastructure.Authorization.Services;

public interface IAuthenticationService
{
    Task<MethodResult<AuthenticateResponse>> Register(RegisterRequest model);
    Task<MethodResult<AuthenticateResponse>> Login(LoginRequest model);
}