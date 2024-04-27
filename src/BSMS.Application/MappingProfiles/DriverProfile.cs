using BSMS.Application.Features.Driver.Queries.GetAll;
using BSMS.Application.Features.Driver.Queries.GetAllFromCompany;
using BSMS.Core.Entities;
using Mapster;

namespace BSMS.Application.MappingProfiles;
class DriverProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Driver, GetAllDriversFromCompanyResponse>()
            .Map(dest => dest.Name, src => $"{src.FirstName} {src.LastName}");

        config.NewConfig<Driver, GetAllDriversResponse>()
            .Map(dest => dest.License, src => src.DriverLicense)
            .Map(dest => dest.CompanyName, src => src.Company.Name);
    }
}