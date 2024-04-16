using BSMS.Core.Entities;
using BSMS.Core.Enums;
using BSMS.Infrastructure.Authorization.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BSMS.API.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizationAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<Role> _roles;

    /// <inheritdoc />
    public AuthorizationAttribute(params Role[]? roles)
    {
        _roles = roles ?? new[] { Role.Admin, Role.Passenger, Role.Passenger };
    }

    /// <inheritdoc />
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var isRolePermission = false;
        var user = (User)context.HttpContext.Items["User"];
        if (user == null)
        {
            context.Result = new JsonResult(new { Message = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }

        if (user != null && _roles.Any())
        {
            foreach (var authRole in _roles)
            { 
                if (user.Role == authRole) 
                { 
                    isRolePermission = true;
                }
            }
        }
            
        if (!isRolePermission)
        {
            context.Result = new JsonResult(new { Message = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}