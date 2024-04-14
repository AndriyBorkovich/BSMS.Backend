namespace BSMS.Infrastructure.Authorization.Models;

public class AuthenticateResponse
{
    public string UserName { get; set; }
    public string Token { get; set; }
}