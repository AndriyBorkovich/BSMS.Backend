using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Helpers;
using FluentValidation;

namespace BSMS.Application.Features.Driver.Commands.Create;

public class CreateDriverCommandValidator : AbstractValidator<CreateDriverCommand>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IBusRepository _busRepository;
    public CreateDriverCommandValidator(ICompanyRepository companyRepository, IBusRepository busRepository)
    {
        _companyRepository = companyRepository;
        _busRepository = busRepository;
        
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

        RuleFor(c => c.BusId)
            .MustAsync(BusExists)
            .WithMessage("Chosen bus must exist!");
    }

    private Task<bool> CompanyExists(int? companyId, CancellationToken cancellationToken)
    {
        return companyId is not null ? _companyRepository.AnyAsync(c => c.CompanyId == companyId) : Task.FromResult(true);
    }
    
    private Task<bool> BusExists(int? busId, CancellationToken cancellationToken)
    {
        return busId is not null ? _busRepository.AnyAsync(b => b.BusId == busId) : Task.FromResult(true);
    }
}