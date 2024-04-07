using BSMS.Application.Features.Common;
using BSMS.Application.Helpers;
using BSMS.Core.Enums;
using MediatR;

namespace BSMS.Application.Features.Bus.Commands.Create;

public record CreateBusCommand(
    string Brand,
    int Capacity,
    string Number,
    int? DriverId,
    List<CreateBusSchedule> BusScheduleEntries) : IRequest<MethodResult<CreatedEntityResponse>>;

public record CreateBusSchedule(
    int RouteId,
    TimeOnly DepartureTime,
    TimeOnly ArrivalTime,
    Direction MoveDirection,
    DayOfWeek DayOfWeek);