using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using FluentValidation;

namespace BSMS.Application.Features.Bus.Commands.Create;

public class CreateBusCommandValidator : AbstractValidator<CreateBusCommand>
{
    private readonly IRouteRepository _routeRepository;
    private readonly IDriverRepository _driverRepository;
    public CreateBusCommandValidator(
        IRouteRepository routeRepository, 
        IDriverRepository driverRepository)
    {
        _routeRepository = routeRepository;
        _driverRepository = driverRepository;

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

        RuleFor(c => c.DriverId)
            .MustAsync(DriverExists)
            .WithMessage("Driver must exists in DB");
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
        var enumerable = groupedSchedules.ToList();
        
        return !enumerable.Exists(group => HasTimeIntersectionsInGroup(group.ToList()));
    }

    private async Task<bool> DriverExists(int driverId, CancellationToken cancellationToken)
    {
        return await _driverRepository.AnyAsync(d => d.DriverId == driverId);
    }

    private bool HasTimeIntersectionsInGroup(List<CreateBusSchedule> schedules)
    {
        for (var i = 0; i < schedules.Count - 1; i++)
        {
            for (var j = i + 1; j < schedules.Count; j++)
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