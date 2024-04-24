using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using FluentValidation;

namespace BSMS.Application.Features.Passenger.Commands.Edit;

public class EditPassengerCommandValidator : AbstractValidator<EditPassengerCommand>
{
    private readonly IPassengerRepository _passengerRepository;
    public EditPassengerCommandValidator(IPassengerRepository passengerRepository)
    {
        _passengerRepository = passengerRepository;

        RuleFor(c => c.PassengerId)
            .MustAsync((id, _) => passengerRepository.AnyAsync(p => p.PassengerId == id))
            .WithMessage("Passenger must exist!");

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