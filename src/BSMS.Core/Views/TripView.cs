namespace BSMS.Core.Views;

public class TripView
{
    public int TripId { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string RouteName { get; set; }
    public string BusBrand { get; set; }
    public string CompanyName { get; set; }
    public int BusRating { get; set; }
    public string TripStatus { get; set; }
    public int FreeSeatsCount { get; set; }
}
