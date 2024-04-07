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

        RuleForEach(c => c.BusScheduleEntries)
            .Must(entry => entry.DepartureTime < entry.ArrivalTime)
            .WithMessage("Schedule must have correct time gaps");
        
        RuleFor(c => c.BusScheduleEntries)
            .Must(HaveNoTimeIntersections)
            .WithMessage("The bus schedule can't have time intersections");
    }

    private async Task<bool> HaveValidRoutesForSchedule(CreateBusCommand command, CancellationToken cancellationToken)
    {
        foreach (var entry in command.BusScheduleEntries)
        {
            if (!await _routeRepository.AnyAsync(r => r.RouteId == entry.RouteId))
            {
                return false;
            }
        }
        
        return true;
    }
    
    private bool HaveNoTimeIntersections(List<CreateBusSchedule> newBusSchedules)
    {
        var groupedSchedules = newBusSchedules.GroupBy(schedule => schedule.DayOfWeek);

        return !groupedSchedules.Any(group => HasTimeIntersectionsInGroup(group.ToList())) && groupedSchedules.Any(g => HasTimeIntersectionsInGroup(g.ToList()));
    }

    private bool HasTimeIntersectionsInGroup(List<CreateBusSchedule> schedules)
    {
        for (int i = 0; i < schedules.Count - 1; i++)
        {
            for (int j = i + 1; j < schedules.Count; j++)
            {
                if (TimeIntersects(schedules[i].DepartureTime, schedules[i].ArrivalTime,
                        schedules[j].DepartureTime, schedules[j].ArrivalTime))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool TimeIntersects(TimeOnly startTime1, TimeOnly endTime1, TimeOnly startTime2, TimeOnly endTime2)
    {
        return startTime1 < endTime2 && endTime1 > startTime2;
    }
}