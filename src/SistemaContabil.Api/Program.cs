using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using SistemaContabil.Api.Endpoints;
using SistemaContabil.Infrastructure.Data;
using SistemaContabil.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SistemaContabilDb>(opt => opt.UseInMemoryDatabase("SistemaContabilDb"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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
    });

builder.Services.AddHealthChecks()
    .AddOracle(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        name: "CoreMonitor-API",
        failureStatus: HealthStatus.Degraded,
        tags: ["CM", "Oracle"],
        healthQuery: "SELECT 1 FROM DUAL",
        timeout: TimeSpan.FromSeconds(30)
    );

builder.Services.AddHealthChecksUI(options =>
{
    options.SetEvaluationTimeInSeconds(150);
    options.MaximumHistoryEntriesPerEndpoint(5);
    options.SetApiMaxActiveRequests(1);
    options.AddHealthCheckEndpoint("api", "/health");
}).AddInMemoryStorage();

// Configuração do Swagger/OpenAPI/Scalar
builder.Services.AddOpenApi();

// Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddProblemDetails();

// Configuração de validação
builder.Services.AddProblemDetails();

var app = builder.Build();

// Middleware de tratamento de erros
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Middleware de logging de requisições
app.UseMiddleware<RequestLoggingMiddleware>();

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