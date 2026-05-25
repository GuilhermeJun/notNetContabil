using HealthChecks.UI.Client;
using IdempotentAPI.Cache.DistributedCache.Extensions.DependencyInjection;
using IdempotentAPI.Core;
using IdempotentAPI.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using System.Data.Common;
using System.Text.Json.Serialization;
using SistemaContabil.Api.Endpoints;
using SistemaContabil.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura DbContext com logging detalhado do EF para diagnosticar queries/erros de schema
builder.Services.AddDbContext<SistemaContabilDb>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("FiapOracle"));
    options.LogTo(Console.WriteLine, LogLevel.Information);
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
});


builder.Services.AddIdempotentAPI();
builder.Services.AddIdempotentMinimalAPI(new IdempotencyOptions());
builder.Services.AddDistributedMemoryCache();
builder.Services.AddIdempotentAPIUsingDistributedCache();


// Configuração do Serilog (logging estruturado)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "SistemaContabil")
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/sistema-contabil-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        // Configurar para aceitar string de 1 caractere como char
        options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
        // Evita loops de referência ao serializar entidades do EF Core
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


builder.Services.AddHealthChecks()
    .AddOracle(
        connectionString: builder.Configuration.GetConnectionString("FiapOracle"),
        name: "CoreMonitor-API",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "CM", "Oracle" },
        healthQuery: "SELECT 1 FROM DUAL",
        timeout: TimeSpan.FromSeconds(30)
    );


// Configuração do Swagger/OpenAPI/Scalar
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();

var app = builder.Build();

// Serilog request logging para correlação e logging estruturado
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        diagnosticContext.Set("RequestPath", httpContext.Request.Path);
        diagnosticContext.Set("RequestMethod", httpContext.Request.Method);
        diagnosticContext.Set("CorrelationId", httpContext.TraceIdentifier);
    };
    options.MessageTemplate = "Handled {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Sistema Contábil - API";
        options.Theme = ScalarTheme.DeepSpace;
    });
}

app.RegisterClienteEndpoints();
app.RegisterContaEndpoints();
app.RegisterRegistroContabilEndpoints();
app.RegisterProdutoEndpoints();
app.RegisterPagamentoEndpoints();
app.RegisterVendaEndpoints();
app.RegisterItemVendaEndpoints();

// Health check endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

// Test endpoint
app.MapGet("/", () => Results.Ok(new { 
    Message = "Sistema Contábil API está funcionando!",
    Scalar = "/scalar/v1",
    Health = "/health",
    Timestamp = DateTime.UtcNow
})).WithName("Root").WithTags("Test");

app.Run();
