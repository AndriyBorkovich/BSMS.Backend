using BSMS.Application.Contracts;
using FluentValidation;

namespace BSMS.Application.Features.Bus.Commands.Create;

public class CreateBusCommandValidator : AbstractValidator<CreateBusCommand>
{
    private readonly IRouteRepository _routeRepository;
    public CreateBusCommandValidator(IRouteRepository routeRepository)
    {
        _routeRepository = routeRepository;

        RuleFor(c => c)
            .MustAsync(HaveValidRoutesForSchedule)
            .WithMessage("Invalid routes selected");
    }
    
    private async Task<bool> HaveValidRoutesForSchedule(CreateBusCommand command, CancellationToken cancellationToken)
    {
        foreach (var scheduleEntry in command.BusScheduleEntries)
        {
            if (!await _routeRepository.AnyAsync(r => r.RouteId == scheduleEntry.RouteId))
            {
                return false;
            }
        }
        
        return true;
    }
}