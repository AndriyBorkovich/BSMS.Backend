using System.ComponentModel.DataAnnotations;

namespace BSMS.Core.Entities;

public class Company
{
    public int CompanyId { get; set; }
    
    [StringLength(50)]
    public string Name { get; set; } = null!;
    [StringLength(100)]
    public string Address { get; set; } = null!;
    [StringLength(20)]
    public string? ContactPhone { get; set; }
    [StringLength(50)]
    public string? ContactEmail { get; set; }
    
    public List<Driver> Drivers { get; set; }
}
