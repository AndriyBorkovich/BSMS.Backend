using BSMS.Application.Contracts.Persistence;
using BSMS.Infrastructure.Persistence;
using BSMS.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BSMS.Infrastructure;

public static class PersistenceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BusStationContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DefaultLocal"));
        });

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IBusRepository, BusRepository>();

        return services;
    }
}