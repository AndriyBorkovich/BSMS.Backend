using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Core.Entities;

[Index(nameof(FirstName))]
[Index(nameof(LastName))]
public class Driver
{
    public int DriverId { get; set; }
    public int CompanyId { get; set; }
    
    [StringLength(50)]
    public string FirstName { get; set; } = null!;
    [StringLength(50)]
    public string LastName { get; set; } = null!;
    [StringLength(50)]
    public string? DriverLicense { get; set; }
    
    public List<Bus> Buses { get; set; }
    public Company Company { get; set; }
}
