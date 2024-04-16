using System.ComponentModel.DataAnnotations;
using BSMS.Core.Enums;

namespace BSMS.Core.Entities;
/// <summary>
/// Clone of DB-level user with his role
/// </summary>
public class User
{
    public int UserId { get; set; }
    [StringLength(50)]
    public string Username { get; set; }
    [StringLength(50)]
    public string Email { get; set; }
    public Role Role { get; set; }
    public DateTime LastLoginDate { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}