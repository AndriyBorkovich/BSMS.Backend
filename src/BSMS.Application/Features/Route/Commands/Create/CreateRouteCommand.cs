using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using MediatR;

namespace BSMS.Application.Features.Route.Commands.Create;

public record CreateRouteCommand(
    string Origin,
    string Destination,
    List<CreateStops> StopsList) : IRequest<MethodResult<CreatedEntityResponse>>;

public record CreateStops (
    string Name);