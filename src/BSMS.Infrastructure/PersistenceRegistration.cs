using BSMS.Application.Contracts.Persistence;
using BSMS.Infrastructure.Persistence;
using BSMS.Infrastructure.Persistence.Interceptors;
using BSMS.Infrastructure.Persistence.Repositories;
using BSMS.Infrastructure.Persistence.Triggers;
using BSMS.Infrastructure.Seed;
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
        const int commandTimeoutInSeconds = 120;
        services.AddDbContext<BusStationContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DefaultLocal"), options =>
            {
                options.CommandTimeout(commandTimeoutInSeconds);
            }).LogTo(Log.Logger.Information, LogLevel.Information);
            //.AddInterceptors(new ExecuteAsCommandInterceptor()); // turn off when doing migrations and seed

            opt.UseTriggers(configuration =>
            {
                configuration.AddTrigger<BusChangeTrigger>();
                configuration.AddTrigger<DriverChangeTrigger>();
                configuration.AddTrigger<PassengerChangeTrigger>();
                configuration.AddTrigger<CompanyChangeTrigger>();
                configuration.AddTrigger<TripEventsTrigger>();
                configuration.AddTrigger<TicketStatusChangeTrigger>();
            });

            opt.EnableDetailedErrors();
            opt.EnableSensitiveDataLogging();
        });

        AddRepositories(services);

        services.AddSingleton<IBusStationSeeder, BusStationSeeder>();

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
        services.AddScoped<ITicketPaymentRepository, TicketPaymentRepository>();
    }
}