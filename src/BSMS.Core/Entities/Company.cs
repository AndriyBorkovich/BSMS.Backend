namespace BSMS.Core.Entities;

public class Company
{
    public int CompanyId { get; set; }
    
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    
    public List<Driver> Drivers { get; set; }
}
