using BSMS.Application.Helpers;
using FluentValidation;

namespace BSMS.Application.Features.Company.Commands.Create;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .NotNull()
            .Length(3, 50)
            .Matches(RegexConstants.LettersOnly)
            .WithMessage("{PropertyName} must consist only from letters");
        
        RuleFor(c => c.Phone)
            .NotEmpty()
            .Matches(RegexConstants.PhoneNumber)
            .WithMessage("Invalid phone number format");

        RuleFor(c => c.Email)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(RegexConstants.Email)
            .WithMessage("Invalid email format");
    }
}