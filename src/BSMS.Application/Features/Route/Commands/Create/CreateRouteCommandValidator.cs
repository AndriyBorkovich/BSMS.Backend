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
            .WithMessage("Invalid path for stops list.");
    }

    private static bool HaveValidPath(CreateRouteCommand routeCommand)
    {
        var stops = routeCommand.StopsList;
        if (stops.Count < 2)
            return false;

        if (stops[0].Name != routeCommand.Origin)
            return false;
        
        return stops[^1].Name == routeCommand.Destination;
    }
}