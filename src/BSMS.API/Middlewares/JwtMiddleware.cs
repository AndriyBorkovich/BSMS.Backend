using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BSMS.Infrastructure.Authorization;
using BSMS.Infrastructure.Authorization.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BSMS.API.Middlewares;

public class JwtMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtSettings)
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            await AttachUserToContext(context, userService, token);
        }
        
        await next(context);
    }

    private async Task AttachUserToContext(HttpContext context, IUserService userService, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience
        }, out var validateToken);


        var jwtToken = (JwtSecurityToken)validateToken;
        var userId = int.Parse(jwtToken.Claims.FirstOrDefault(_=>_.Type== JwtRegisteredClaimNames.NameId).Value);
        context.Items["User"] = await userService.GetByIdAsync(userId);
    }
}