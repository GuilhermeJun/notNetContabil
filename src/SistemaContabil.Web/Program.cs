using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using SistemaContabil.Application.Extensions;
using SistemaContabil.Infrastructure.Extensions;
using SistemaContabil.Web.Endpoints;
using SistemaContabil.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

/*
#region Database
builder.Services.AddDbContext<SistemaContabilDbContext>(
    options => options.UseInMemoryDatabase("TodoDb"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
#endregion
*/

// Configuração do Serilog (logging estruturado)
Log.Logger = new LoggerConfiguration()
    // Nível mínimo aplicável ao logger
    .MinimumLevel.Information()
    // Elevar nível de comentários de bibliotecas do framework para Warning
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "SistemaContabil")
    // Saída estruturada para console (texto legível) e arquivo (rolling)
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/sistema-contabil-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    // Permitir sobrescrever via configuration (appsettings.json)
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
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo()
        {
            Title = "CoreMonitor - Sistema Contábil API",
            Version = "1.0.1",
            Description = """API para sistema contábil com Oracle Database""",
            License = new OpenApiLicense()
            {
                Name = "Core Monitor",
                Url = new Uri("https://coremonitor.com.br/licence/coremonitor/")
            },
            Contact = new OpenApiContact()
            {
                Email = "contato@test.com.br",
                Name = "CoreMonitor",
                Url = new Uri("https://www.fiap.com.br")
            }
        };
        return Task.CompletedTask;
    }
    );
}
);

/*
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema Contábil API",
        Version = "v1",
        Description = "API para sistema contábil com Oracle Database",
        Contact = new OpenApiContact
        {
            Name = "FIAP - Sistema Contábil",
            Email = "contato@fiap.com.br"
        }
    });

    // Incluir comentários XML
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});
*/

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

// Configuração de serviços das camadas
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddProblemDetails();

// Configuração de validação
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Swagger sempre disponível para facilitar testes
/*
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema Contábil API v1");
    c.RoutePrefix = "swagger"; // Swagger UI em /swagger
    c.DisplayRequestDuration();
    c.EnableDeepLinking();
    c.EnableFilter();
    c.ShowExtensions();
});
*/

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

// Adicionar cabeçalho de correlação na resposta
app.Use(async (context, next) =>
{
    var correlationId = context.TraceIdentifier;
    if (!context.Response.Headers.ContainsKey("X-Correlation-ID"))
    {
        context.Response.Headers.Add("X-Correlation-ID", correlationId);
    }
    await next();
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

// Mapear rotas MVC - priorizar controllers do namespace Mvc
app.MapControllerRoute(
    name: "mvc",
    pattern: "{controller=Home}/{action=Index}/{id?}",
    defaults: null,
    constraints: null,
    dataTokens: new { area = (string?)null, namespaces = new[] { "SistemaContabil.Web.Controllers.Mvc", "SistemaContabil.Web.Controllers" } });

app.MapControllers();

// Mapear endpoints de busca paginada
app.MapSearchEndpoints();

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

app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Test endpoint
app.MapGet("/", () => Results.Ok(new { 
    Message = "Sistema Contábil API está funcionando!",
    Scalar = "/scalar/v1",
    Health = "/health",
    Timestamp = DateTime.UtcNow
}))
    .WithName("Root")
    .WithTags("Test");


try
{
    Log.Information("Iniciando Sistema Contábil API");
    Log.Information("Testando conexões Oracle...");
    var workingConnection = await SistemaContabil.Infrastructure.Configuration.ConnectionTester.TestConnectionsAsync();
    
    if (workingConnection != null)
    {
        Log.Information("✅ Conexão Oracle funcionando!");
    }
    else
    {
        Log.Warning("⚠️ Nenhuma conexão Oracle funcionou. Verifique as configurações.");
    }
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplicação encerrada inesperadamente");
}
finally
{
    Log.CloseAndFlush();
}
