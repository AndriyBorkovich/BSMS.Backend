using BSMS.Application.Features.Route.Commands.Create;
using BSMS.Application.Features.Route.Queries.GetAll;
using BSMS.Core.Entities;
using Mapster;

namespace BSMS.Application.MappingProfiles;

public class RouteProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Route, GetAllRoutesResponse>()
            .Map(dest => dest.Name, src => $"{src.Origin} - {src.Destination}");
    }
}