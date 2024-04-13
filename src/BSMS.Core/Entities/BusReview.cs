using System.ComponentModel.DataAnnotations;

namespace BSMS.Core.Entities;

public class BusReview
{
    public int BusReviewId { get; set; }
    public int BusId { get; set; }
    public int PassengerId { get; set; }
    
    [Range(1, 5)]
    public int ComfortRating { get; set; }
    [Range(1, 5)]
    public int PunctualityRating { get; set; }
    [Range(1, 5)]
    public int PriceQualityRatioRating { get; set; }
    [Range(1, 5)]
    public int InternetConnectionRating { get; set; }
    [Range(1, 5)]
    public int SanitaryConditionsRating { get; set; }
    [StringLength(200)]
    public string? Comments { get; set; }
    public DateTime ReviewDate { get; set; }
    
    public Bus Bus { get; set; }
    public Passenger Passenger { get; set; }
}
