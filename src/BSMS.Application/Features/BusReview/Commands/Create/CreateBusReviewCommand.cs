using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using MediatR;

namespace BSMS.Application.Features.BusReview.Commands.Create;

public record CreateBusReviewCommand(
    int BusId,
    int PassengerId,
    int ComfortRating,
    int PunctualityRating,
    int PriceQualityRatioRating,
    int InternetConnectionRating,
    int SanitaryConditionsRating,
    string? Comments) : IRequest<MethodResult<CreatedEntityResponse>>;