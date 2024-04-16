using BSMS.Infrastructure.Authorization.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BSMS.Infrastructure;

public static class IdentityRegistration
{
    public static void AddCustomIdentityServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }
}