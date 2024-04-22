using BSMS.API;
using BSMS.Application;
using BSMS.Infrastructure;
using BSMS.API.Extensions;
using BSMS.API.Middlewares;
using BSMS.Infrastructure.Authorization;
using Serilog;
using Microsoft.AspNetCore.HttpLogging;
using BSMS.API.BackgroundJobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddLogging(loggingBuilder =>
    loggingBuilder.AddSerilog(dispose: true));

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

// exceptions handling
builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Extensions.Add("trace-id", ctx.HttpContext.TraceIdentifier);
        ctx.ProblemDetails.Extensions.Add("instance", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
    }
);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddRedisCaching(builder.Configuration);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddPersistenceServices(builder.Configuration)
                .AddApplicationServices()
                .AddCustomIdentityServices();

builder.Services.AddHostedService<CacheCleaningJob>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("All",
        policyConfig => policyConfig.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("All");

app.UseExceptionHandler();

app.UseSerilogRequestLogging();

app.UseMiddleware<JwtMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();