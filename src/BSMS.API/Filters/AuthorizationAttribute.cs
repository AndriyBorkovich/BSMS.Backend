using BSMS.Core.Entities;
using BSMS.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BSMS.API.Filters;

/// <inheritdoc />
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizationAttribute(params Role[]? roles) : Attribute, IAuthorizationFilter
{
    private readonly IList<Role> _roles = roles ?? new[] { Role.Admin, Role.Passenger, Role.Passenger };

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