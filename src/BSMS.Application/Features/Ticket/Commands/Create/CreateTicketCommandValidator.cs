using BSMS.Application.Contracts.Persistence;
using FluentValidation;

namespace BSMS.Application.Features.Ticket.Commands.Create;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    private ISeatRepository _seatRepository;
    private IStopRepository _stopRepository;
    public CreateTicketCommandValidator(ISeatRepository seatRepository, IStopRepository stopRepository)
    {
        _seatRepository = seatRepository;
        _stopRepository = stopRepository;

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