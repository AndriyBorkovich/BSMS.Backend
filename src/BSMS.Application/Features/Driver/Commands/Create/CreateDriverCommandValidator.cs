using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using FluentValidation;

namespace BSMS.Application.Features.Driver.Commands.Create;

public class CreateDriverCommandValidator : AbstractValidator<CreateDriverCommand>
{
    private readonly ICompanyRepository _companyRepository;
    public CreateDriverCommandValidator(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
        
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
            .MustAsync(CompanyExists)
            .WithMessage("Chosen company must exist!");
    }

    private Task<bool> CompanyExists(int companyId, CancellationToken cancellationToken)
    {
        return _companyRepository.AnyAsync(c => c.CompanyId == companyId);
    }
}