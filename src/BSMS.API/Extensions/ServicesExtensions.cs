﻿using System.Reflection;
using System.Text;
using BSMS.API.BackgroundJobs;
using BSMS.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BSMS.API.Extensions;

internal static class ServicesExtensions
{
    public static void AddCustomizedProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
            options.CustomizeProblemDetails = ctx =>
            {
                ctx.ProblemDetails.Extensions.Add("trace-id", ctx.HttpContext.TraceIdentifier);
                ctx.ProblemDetails.Extensions.Add("instance", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
            }
        );
    }

    /// <summary>
    /// Configure Swagger OpenAPI 
    /// </summary>
    /// <param name="services">Extended class</param>
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Bus station API",
                Description = "An API for managing bus station",
                Version = "v1"
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

    /// <summary>
    /// Configure JWT authorization
    /// </summary>
    /// <param name="services">Extended class</param>
    /// <param name="configuration">Configuration app settings</param>
    public static void AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidIssuer = jwtSettings.Issuer
            };
        });
    }

    /// <summary>
    /// Add one-time running and recurring jobs
    /// </summary>
    /// <param name="services"></param>
    public static void AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<CacheCleaningJob>();
        //services.AddHostedService<DatabaseSeedJob>(); // comment if you have already filled DB
        services.AddHostedService<ScheduleTripsJob>();
        services.AddHostedService<TripStartOrStopPeriodicJob>();
        services.AddHostedService<TicketGenerationJob>();
    }
}