namespace BSMS.Core.Entities;

public class Bus
{
    public int BusId { get; set; }
    
    public int Capacity { get; set; }
    public string Brand { get; set; } = null!;
    public string Number { get; set; } = null!;

    /// <summary>
    /// helps to get bus schedule, trip statistic
    /// </summary>
    public List<BusScheduleEntry> BusScheduleEntries { get; set; }
    public List<BusReview> BusReviews { get; set; }
    public List<Driver> Drivers { get; set; }
    public List<Passenger> Passengers { get; set; }
    public List<Seat> Seats { get; set; }
}
