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
    }
}