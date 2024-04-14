using BSMS.Core.Enums;

namespace BSMS.Infrastructure.Authorization.Models;

public class RegisterRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}