using System.Reflection;
using BSMS.Application.Features.Bus.Commands.Create;
using BSMS.Application.Features.Company.Commands.Create;
using BSMS.Application.Features.Driver.Commands.Create;
using BSMS.Application.Features.Passenger.Commands.Create;
using BSMS.Application.Features.Route.Commands.Create;
using BSMS.Application.Helpers;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace BSMS.Application;

public static class ApplicationRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<MethodResultFactory>();
        
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(mappingConfig);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        AddValidators(services);

        return services;
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateRouteCommand>, CreateRouteCommandValidator>();
        services.AddScoped<IValidator<CreateBusCommand>, CreateBusCommandValidator>();
        services.AddScoped<IValidator<CreatePassengerCommand>, CreatePassengerCommandValidator>();
        services.AddScoped<IValidator<CreateDriverCommand>, CreateDriverCommandValidator>();
        services.AddScoped<IValidator<CreateCompanyCommand>, CreateCompanyCommandValidator>();
    }
}