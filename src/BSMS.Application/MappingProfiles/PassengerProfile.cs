using BSMS.Application.Features.Passenger.Queries.GetAll;
using BSMS.Core.Entities;
using Mapster;

namespace BSMS.Application.MappingProfiles;

public class PassengerProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Passenger, GetAllPassengersResponse>()
            .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");
    }
}