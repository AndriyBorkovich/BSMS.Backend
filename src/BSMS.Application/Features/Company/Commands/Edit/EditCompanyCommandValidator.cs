using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Features.Company.Commands.Edit;
using BSMS.Application.Helpers;
using FluentValidation;

namespace BSMS.Application;

public class EditCompanyCommandValidator : AbstractValidator<EditCompanyCommand>
{
    private readonly ICompanyRepository _companyRepository;
    public EditCompanyCommandValidator(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;

        RuleFor(c => c.CompanyId)
            .MustAsync(async (id, _) => await _companyRepository.AnyAsync(c => c.CompanyId == id))
            .WithMessage("Choosen company must exist!");

        RuleFor(c => c.Name)
            .NotEmpty()
            .NotNull()
            .Length(3, 50)
            .Matches(RegexConstants.LettersOnly)
            .WithMessage("{PropertyName} must consist only from letters");

        RuleFor(c => c.Street)
           .NotEmpty()
           .NotNull()
           .Length(5, 50)
           .Matches(RegexConstants.Street)
           .WithMessage("{PropertyName} is invalid");

        RuleFor(c => c.City)
            .NotEmpty()
            .NotNull()
            .Length(3, 50)
            .Matches(RegexConstants.City)
            .WithMessage("{PropertyName} is invalid");

        RuleFor(c => c.Country)
            .NotEmpty()
            .NotNull()
            .Length(3, 50)
            .Matches(RegexConstants.LettersOnly)
            .WithMessage("{PropertyName} must consist only from letters");

        RuleFor(c => c.ZipCode)
            .NotEmpty()
            .NotNull()
            .Length(5, 10)
            .Matches(RegexConstants.ZipCode)
            .WithMessage("{PropertyName} is invalid");

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
