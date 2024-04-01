namespace BSMS.Core.Entities;

public class BusReview
{
    public int BusReviewId { get; set; }
    public int BusId { get; set; }
    public int PassengerId { get; set; }
    
    public int? ComfortRating { get; set; }
    public int? PunctualityRating { get; set; }
    public int? PriceQualityRatioRating { get; set; }
    public int? InternetConnectionRating { get; set; }
    public int? SanitaryConditionsRating { get; set; }
    public string? Comments { get; set; }
    public DateTime ReviewDate { get; set; }
    
    public Bus Bus { get; set; }
    public Passenger Passenger { get; set; }
}
