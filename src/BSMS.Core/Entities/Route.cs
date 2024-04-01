namespace BSMS.Core.Entities;

public class Route
{
    public int RouteId { get; set; }
    public string Origin { get; set; } = null!;
    public string Destination { get; set; } = null!;

    public List<Stop> Stops { get; set; }
    public List<BusScheduleEntry> BusScheduleEntries { get; set; }
}
