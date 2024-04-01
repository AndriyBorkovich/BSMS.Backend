namespace BSMS.Core.Entities;

public class Driver
{
    public int DriverId { get; set; }
    public int? CompanyId { get; set; }
    public int? BusId { get; set; }
    
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? DriverLicense { get; set; }
    
    public Bus? Bus { get; set; }
    public Company? Company { get; set; }
}
