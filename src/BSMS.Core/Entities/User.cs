using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BSMS.Core.Enums;

namespace BSMS.Core.Entities;

public class User
{
    public int UserId { get; set; }
    [StringLength(50)]
    public string Username { get; set; }
    [StringLength(50)]
    public string Email { get; set; }
    public Role Role { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}