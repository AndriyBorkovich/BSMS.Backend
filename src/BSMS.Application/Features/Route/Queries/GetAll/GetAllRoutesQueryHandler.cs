using BSMS.Application.Contracts.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BSMS.Application.Features.Route.Queries.GetAll;
public record GetAllRoutesQuery() : IRequest<List<GetAllRoutesResponse>>;

public record GetAllRoutesResponse(
    int RouteId,
    string Name
);

public class GetAllRoutesQueryHandler(
    IRouteRepository repository) : IRequestHandler<GetAllRoutesQuery, List<GetAllRoutesResponse>>
{
    public async Task<List<GetAllRoutesResponse>> Handle(
        GetAllRoutesQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAll()
                               .AsNoTracking()
                               .ProjectToType<GetAllRoutesResponse>()
                               .ToListAsync(cancellationToken);
    }
}
