using BSMS.API;
using BSMS.Application;
using BSMS.Infrastructure;
using BSMS.API.Extensions;
using BSMS.API.Middlewares;
using BSMS.Infrastructure.Authorization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
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

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration)
                .AddApplicationServices()
                .AddCustomIdentityServices();


builder.Services.AddCors(options =>
{
    options.AddPolicy("All",
        policyConfig => policyConfig.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Enable CORS
app.UseCors("All");

// Use exception handling middleware
app.UseExceptionHandler();

// Use Serilog request logging
app.UseSerilogRequestLogging();

// Use JWT middleware
app.UseMiddleware<JwtMiddleware>();

// Enable Swagger UI (only in development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Enable authentication
app.UseAuthentication();

// Enable authorization
app.UseAuthorization();

app.UseRouting();
// Map controllers
app.MapControllers();

// Run the application
app.Run();