using BSMS.Application.Features.Company.Commands.Create;
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
    }
}