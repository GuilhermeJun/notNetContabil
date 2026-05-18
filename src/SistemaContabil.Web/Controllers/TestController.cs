using Microsoft.AspNetCore.Mvc;
using SistemaContabil.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;

namespace SistemaContabil.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    [HttpGet("connection")]
    public async Task<ActionResult<object>> TestConnection()
    {
        try
        {
            _logger.LogInformation("Iniciando teste de conexões Oracle...");

            var workingConnection = await ConnectionTester.TestConnectionsAsync(_logger);

            if (workingConnection != null)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Conexão Oracle funcionando!",
                    ConnectionString = workingConnection.Replace("Password=310589;", "Password=***;"),
                    Timestamp = DateTime.UtcNow
                });
            }
            else
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Nenhuma string de conexão funcionou",
                    TestedConnections = DatabaseConfiguration.ConnectionStrings.Length,
                    Timestamp = DateTime.UtcNow
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao testar conexões");
            return StatusCode(500, new
            {
                Success = false,
                Message = "Erro interno ao testar conexões",
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("connections")]
    public ActionResult<object> ListConnections()
    {
        var connections = DatabaseConfiguration.ConnectionStrings
            .Select((cs, index) => new
            {
                Index = index + 1,
                ConnectionString = cs.Replace("Password=310589;", "Password=***;"),
                Description = GetConnectionDescription(cs)
            })
            .ToArray();

        return Ok(new
        {
            TotalConnections = connections.Length,
            Connections = connections,
            Timestamp = DateTime.UtcNow
        });
    }

    private static string GetConnectionDescription(string connectionString)
    {
        if (connectionString.Contains("/orcl") && !connectionString.Contains("Connection Timeout"))
            return "Service Name /orcl (FORMATO CORRETO)";
        if (connectionString.Contains("/ORCL") && !connectionString.Contains("Connection Timeout"))
            return "Service Name /ORCL (maiúsculo)";
        if (connectionString.Contains("/orcl") && connectionString.Contains("Connection Timeout"))
            return "Service Name /orcl com timeout";
        if (connectionString.Contains("/ORCL") && connectionString.Contains("Connection Timeout"))
            return "Service Name /ORCL com timeout";
        if (connectionString.Contains("DESCRIPTION") && connectionString.Contains("SERVICE_NAME=orcl"))
            return "Formato TNS com SERVICE_NAME=orcl";
        if (connectionString.Contains("DESCRIPTION") && connectionString.Contains("SERVICE_NAME=ORCL"))
            return "Formato TNS com SERVICE_NAME=ORCL";
        
        return "Formato desconhecido";
    }
}
