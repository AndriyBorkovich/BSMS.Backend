using System.ComponentModel.DataAnnotations;

namespace BSMS.Core.Entities;

public class Bus
{
    public int BusId { get; set; }
    public int DriverId { get; set; }
    public int Capacity { get; set; }
    [StringLength(50)]
    public string Brand { get; set; } = null!;
    [StringLength(20)]
    public string Number { get; set; } = null!;

    /// <summary>
    /// helps to get bus schedule, trip statistics
    /// </summary>
    public List<BusScheduleEntry> BusScheduleEntries { get; set; }
    public List<BusReview> BusReviews { get; set; }
    public Driver Driver { get; set; }
    public List<Seat> Seats { get; set; }
}
