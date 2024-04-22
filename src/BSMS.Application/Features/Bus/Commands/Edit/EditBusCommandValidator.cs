using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using FluentValidation;

namespace BSMS.Application.Features.Bus.Commands.Edit;

class EditBusCommandValidator : AbstractValidator<EditBusCommand>
{
    private readonly IBusRepository _busRepository;
    public EditBusCommandValidator(IBusRepository busRepository)
    {
        _busRepository = busRepository;
        
        RuleFor(c => c.BusId)
            .MustAsync((id, _) => _busRepository.AnyAsync(b => b.BusId == id))
            .WithMessage("Bus with such ID must exists!");
        
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
    }
}