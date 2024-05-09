using System.Data;
using BSMS.Application.Contracts.Persistence;
using FluentValidation;

namespace BSMS.Application.Features.Ticket.Commands.Create;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    private readonly ISeatRepository _seatRepository;
    private readonly IStopRepository _stopRepository;
    private readonly ITripRepository _tripRepository;
    private readonly IPassengerRepository _passengerRepository;
    
    public CreateTicketCommandValidator(
        ISeatRepository seatRepository, 
        IStopRepository stopRepository,
        ITripRepository tripRepository,
        IPassengerRepository passengerRepository)
    {
        _seatRepository = seatRepository;
        _stopRepository = stopRepository;
        _tripRepository = tripRepository;
        _passengerRepository = passengerRepository;

        RuleFor(c => c.TripId)
            .MustAsync(async (id, _) => await _tripRepository.AnyAsync(t => t.TripId == id))
            .WithMessage("Trip must exist!");

        RuleFor(c => c.PassengerId)
            .MustAsync((id, _) => _passengerRepository.AnyAsync(p => p.PassengerId == id))
            .WithMessage("Passenger must exist!");

        RuleFor(c => c.StartStopId)
            .MustAsync(async (id, _) => await _stopRepository.AnyAsync(s => s.StopId == id))
            .WithMessage("Start stop must exist!");

        RuleFor(c => c.EndStopId)
            .MustAsync(async (id, _) => await _stopRepository.AnyAsync(s => s.StopId == id))
            .WithMessage("End stop must exist!");

        RuleFor(c => c)
            .Must(StopsBelongToSameRoute)
            .WithMessage("Chosen stops must belong to the same route");

        RuleFor(c => c.SeatId)
            .MustAsync(async (id, _) => await _seatRepository.AnyAsync(s => s.SeatId == id))
            .WithMessage("Seat must exist!");
    }

    private bool StopsBelongToSameRoute(CreateTicketCommand command)
    {
        return _stopRepository.StopsBelongToSameRoute(command.StartStopId, command.EndStopId);
    }
}