using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Logging;

namespace SistemaContabil.Infrastructure.Configuration;

public static class ConnectionTester
{
    public static async Task<string?> TestConnectionsAsync(ILogger? logger = null)
    {
        foreach (var connectionString in DatabaseConfiguration.ConnectionStrings)
        {
            try
            {
                logger?.LogInformation("Testando conexão: {ConnectionString}", 
                    connectionString.Replace("Password=AppPass#2025;", "Password=***;"));

                using var connection = new OracleConnection(connectionString);
                await connection.OpenAsync();
                
                logger?.LogInformation("✅ Conexão bem-sucedida!");
                return connectionString;
            }
            catch (Exception ex)
            {
                logger?.LogWarning("❌ Falha na conexão: {Error}", ex.Message);
            }
        }

        logger?.LogError("❌ Nenhuma string de conexão funcionou!");
        return null;
    }

    public static async Task<bool> TestConnectionAsync(string connectionString, ILogger? logger = null)
    {
        try
        {
            logger?.LogInformation("Testando conexão: {ConnectionString}", 
                connectionString.Replace("Password=AppPass#2025;", "Password=***;"));

            using var connection = new OracleConnection(connectionString);
            await connection.OpenAsync();
            
            logger?.LogInformation("✅ Conexão bem-sucedida!");
            return true;
        }
        catch (Exception ex)
        {
            logger?.LogWarning("❌ Falha na conexão: {Error}", ex.Message);
            return false;
        }
    }
}
