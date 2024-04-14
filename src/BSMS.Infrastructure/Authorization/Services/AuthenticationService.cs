using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BSMS.Application.Helpers;
using BSMS.Core.Entities;
using BSMS.Infrastructure.Authorization.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BSMS.Infrastructure.Authorization.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserService _userService;
    private readonly MethodResultFactory _methodResultFactory;
    private readonly SymmetricSecurityKey _key;
    private readonly JwtSettings _jwtSettings;

    public AuthenticationService(
        IUserService userService,
        IConfiguration configuration,
        MethodResultFactory methodResultFactory,
        IOptions<JwtSettings> jwtSettings)
    {
        _userService = userService;
        _methodResultFactory = methodResultFactory;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Key!));
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<MethodResult<AuthenticateResponse>> Register(RegisterRequest model)
    {
        var result = _methodResultFactory.Create<AuthenticateResponse>();
        
        if (await _userService.ExistsAsync(model.Username) || await _userService.ExistsAsync(model.Email))
        {
            result.SetError("User with such credentials already exists!", HttpStatusCode.BadRequest);
            return result;
        }

        using var hmac = new HMACSHA512();

        var user = new User() 
        {
            Username = model.Username,
            Role = model.Role,
            Email = model.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password)),
            PasswordSalt = hmac.Key
        };

        await _userService.AddAsync(user);
        
        result.Data = new AuthenticateResponse() 
        {
            Username = user.Username,
            Token = GenerateToken(user)
        };

        return result;
    }

    public async Task<MethodResult<AuthenticateResponse>> Login(LoginRequest model)
    {
        var result = _methodResultFactory.Create<AuthenticateResponse>();
        
        var user = await _userService.GetByUsernameAsync(model.Username);
        if(user is null)
        {
            result.SetError("Such user does not exist!", HttpStatusCode.NotFound);
            return result;
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password));

        if (computedHash.Where((t, i) => t != user.PasswordHash[i]).Any())
        {
            result.SetError("Invalid password!", HttpStatusCode.Forbidden);
            return result;
        }

        result.Data = new AuthenticateResponse() 
        {
            Username = user.Username,
            Token = GenerateToken(user)
        };

        return result;
    }
    
    private string GenerateToken(User user)
    {
        // claims help us to store info about current user
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new(JwtRegisteredClaimNames.NameId, user.UserId.ToString()),
            new("Role", Convert.ToString(user.Role)!)
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = credentials,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}