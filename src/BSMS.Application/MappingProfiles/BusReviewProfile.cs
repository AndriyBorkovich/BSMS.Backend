using BSMS.Application.Features.BusReview.Commands.Create;
using BSMS.Core.Entities;
using Mapster;

namespace BSMS.Application.MappingProfiles;

public class BusReviewProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateBusReviewCommand, BusReview>()
            .AfterMapping((_, dest) =>
            {
                dest.ReviewDate = DateTime.UtcNow;
            });
    }
}