using System.ComponentModel.DataAnnotations;

namespace BSMS.Core.Entities;

public class Route
{
    public int RouteId { get; set; }
    
    [StringLength(50)]
    public string Origin { get; set; } = null!;
    [StringLength(50)]
    public string Destination { get; set; } = null!;

    public int OverallDistance { get; set; }

    public List<Stop> Stops { get; set; }
    public List<BusScheduleEntry> BusScheduleEntries { get; set; }
}
