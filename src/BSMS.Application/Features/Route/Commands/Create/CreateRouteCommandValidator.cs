using FluentValidation;

namespace BSMS.Application.Features.Route.Commands.Create;

public class CreateRouteCommandValidator : AbstractValidator<CreateRouteCommand>
{
    public CreateRouteCommandValidator()
    {
        RuleFor(c => c.Origin)
            .NotNull()
            .NotEmpty();

        RuleFor(c => c.Destination)
            .NotNull()
            .NotEmpty();
        
        RuleFor(command => command)
            .Must(HaveValidPath)
            .WithMessage("Invalid path for stops list. First and last stop is determined by origin and destination of route");
    }

    private static bool HaveValidPath(CreateRouteCommand routeCommand)
    {
        var stops = routeCommand.StopsList;
        
        if (stops[0].Name == routeCommand.Origin)
            return false;
        
        return stops[0].Name != routeCommand.Origin && stops[^1].Name != routeCommand.Destination;
    }
}