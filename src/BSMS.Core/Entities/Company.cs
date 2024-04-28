using System.ComponentModel.DataAnnotations;

namespace BSMS.Core.Entities;

public class Company
{
    public int CompanyId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;
    [StringLength(20)]
    public string? ContactPhone { get; set; }
    [StringLength(50)]
    public string? ContactEmail { get; set; }

    #region Address data
    [StringLength(50)]
    public string Street { get; set; } = null!;
    [StringLength(50)]
    public string City { get; set; } = null!;
    [StringLength(50)]
    public string Country { get; set; } = null!;

    [StringLength(10)]
    public string ZipCode { get; set; } = null!;
    #endregion

    public List<Driver> Drivers { get; set; }
}
