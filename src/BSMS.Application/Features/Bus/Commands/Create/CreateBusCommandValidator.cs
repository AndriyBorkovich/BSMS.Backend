using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using FluentValidation;

namespace BSMS.Application.Features.Bus.Commands.Create;

public class CreateBusCommandValidator : AbstractValidator<CreateBusCommand>
{
    private readonly IRouteRepository _routeRepository;
    public CreateBusCommandValidator(IRouteRepository routeRepository)
    {
        _routeRepository = routeRepository;

        RuleFor(c => c.Brand)
            .NotEmpty()
            .Length(3, 50)
            .Matches(RegexConstants.LettersOnly)
            .WithMessage("{PropertyName} must consist only from letters");

        RuleFor(c => c.Capacity)
            .GreaterThanOrEqualTo(5)
            .LessThanOrEqualTo(30);
        
        RuleFor(c => c.Number)
            .NotEmpty()
            .Length(3, 7)
            .Matches(RegexConstants.LettersAndNumbers)
            .WithMessage("{PropertyName} must consist only from letters and numbers");
        
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