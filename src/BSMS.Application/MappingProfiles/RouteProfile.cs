using BSMS.Application.Features.Route.Commands.Create;
using BSMS.Application.Features.Route.Queries.GetAll;
using BSMS.Core.Entities;
using Mapster;

namespace BSMS.Application.MappingProfiles;

public class RouteProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateRouteCommand, Route>()
            .Map(dest => dest.Stops, src => src.StopsList);

        config.NewConfig<Route, GetAllRoutesResponse>()
            .Map(dest => dest.Name, src => $"{src.Origin} - {src.Destination}");
    }
}