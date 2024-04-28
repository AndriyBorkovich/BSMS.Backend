using BSMS.Application.Features.Company.Commands.Create;
using BSMS.Application.Features.Company.Commands.Edit;
using BSMS.Application.Features.Company.Queries.GetAll;
using BSMS.Core.Entities;
using Mapster;

namespace BSMS.Application.MappingProfiles;

public class CompanyProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCompanyCommand, Company>()
            .Map(dest => dest.ContactPhone, src => src.Phone)
            .Map(dest => dest.ContactEmail, src => src.Email);
        
         config.NewConfig<EditCompanyCommand, Company>()
            .Map(dest => dest.ContactPhone, src => src.Phone)
            .Map(dest => dest.ContactEmail, src => src.Email);

        config.NewConfig<Company, GetAllCompaniesResponse>()
            .Map(dest => dest.Phone, src => src.ContactPhone)
            .Map(dest => dest.Email, src => src.ContactEmail)
            .Map(dest => dest.Address, src => string.Join(", ", new string[] {src.Street, src.City, src.Country, src.ZipCode}));
    }
}