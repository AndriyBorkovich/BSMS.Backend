using BSMS.Application.Features.Route.Commands.Create;
using BSMS.Core.Entities;
using Mapster;

namespace BSMS.Application.MappingProfiles;

public class RouteProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateRouteCommand, Route>()
            .Map(dest => dest.Stops, src => src.StopsList)
            .AfterMapping((src, dest) =>
            {
                // assign stops order
                if (dest.Stops.Count > 1)
                {
                    for (var i = 1; i < dest.Stops.Count; i++)
                    {
                        dest.Stops[i].PreviousStop = dest.Stops[i - 1];
                    }
                }
            });
    }
}