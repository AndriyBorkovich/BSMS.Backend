using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using FluentValidation;

namespace BSMS.Application.Features.Driver.Commands.Edit;

public class EditDriverCommandValidator : AbstractValidator<EditDriverCommand>
{
    private readonly IDriverRepository _driverRepository;
    private readonly ICompanyRepository _companyRepository;
    public EditDriverCommandValidator(
        ICompanyRepository companyRepository, IDriverRepository driverRepository)
    {
        _driverRepository = driverRepository;
        _companyRepository = companyRepository;

        RuleFor(c => c.DriverId)
            .MustAsync((id, _) => _driverRepository.AnyAsync(d => d.DriverId == id))
            .WithMessage("Choosen driver must exist");

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

        RuleFor(x => x.DriverLicense)
            .Matches(RegexConstants.DriverLicense)
            .WithMessage("Invalid driver license format");

        RuleFor(c => c.CompanyId)
            .MustAsync((id, _) => _companyRepository.AnyAsync(c => c.CompanyId == id))
            .WithMessage("Chosen company must exist!");
    }
}