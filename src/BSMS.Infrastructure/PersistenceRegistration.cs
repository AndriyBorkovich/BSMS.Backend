using BSMS.Application.Contracts.Persistence;
using BSMS.Infrastructure.Persistence;
using BSMS.Infrastructure.Persistence.Interceptors;
using BSMS.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BSMS.Infrastructure;

public static class PersistenceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BusStationContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DefaultLocal"))
                .LogTo(Log.Logger.Information, LogLevel.Information)
                .AddInterceptors(new ExecuteAsCommandInterceptor()); // turn off when doing migration
            opt.EnableDetailedErrors();
            opt.EnableSensitiveDataLogging();
        });
        
        AddRepositories(services);

        return services;
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IBusRepository, BusRepository>();
        services.AddScoped<IRouteRepository, RouteRepository>();
        services.AddScoped<IPassengerRepository, PassengerRepository>();
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IBusReviewRepository, BusReviewRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<ISeatRepository, SeatRepository>();
        services.AddScoped<IStopRepository, StopRepository>();
        services.AddScoped<ITripRepository, TripRepository>();
        services.AddScoped<ITripStatusRepository, TripStatusRepository>();
        services.AddScoped<ITicketStatusRepository, TicketStatusRepository>();
        services.AddScoped<ITicketPaymentRepository, TicketPaymentRepository>();
    }
}