using BSMS.Application.Helpers;
using FluentValidation;

namespace BSMS.Application.Features.Passenger.Commands.Create;

public class CreatePassengerCommandValidator : AbstractValidator<CreatePassengerCommand>
{
    public CreatePassengerCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .NotNull()
            .Length(3, 50)
            .Matches(RegexConstants.LettersOnly)
            .WithMessage("{PropertyName} must consist only from letters");
        
        RuleFor(c => c.LastName)
            .NotEmpty()
            .NotNull()
            .Length(3, 50)
            .Matches(RegexConstants.LettersOnly)
            .WithMessage("{PropertyName} must consist only from letters");
        
        RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .Matches(RegexConstants.PhoneNumber)
            .WithMessage("Invalid phone number format");

        RuleFor(c => c.Email)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(RegexConstants.Email).WithMessage("Invalid email format");
    }
}