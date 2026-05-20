using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Hosting;

namespace SistemaContabil.Web.Middleware;

/// Middleware para tratamento global de exceções
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado ocorreu: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Base response
        var errorObj = new Dictionary<string, object?>
        {
            ["message"] = exception.Message,
            ["details"] = exception.InnerException?.Message,
            ["timestamp"] = DateTime.UtcNow,
            ["path"] = context.Request.Path,
            ["method"] = context.Request.Method
        };

        // Include detailed debug info only in Development
        if (_env.IsDevelopment())
        {
            errorObj["exceptionType"] = exception.GetType().FullName;
            errorObj["stackTrace"] = exception.StackTrace;
        }

        var response = new { error = errorObj };

        context.Response.StatusCode = exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,
            InvalidOperationException => (int)HttpStatusCode.BadRequest,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            NotImplementedException => (int)HttpStatusCode.NotImplemented,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}
