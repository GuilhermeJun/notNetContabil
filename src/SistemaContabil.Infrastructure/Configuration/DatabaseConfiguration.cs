using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaContabil.Domain.Interfaces;
using SistemaContabil.Infrastructure.Data;
using SistemaContabil.Infrastructure.Repositories;

namespace SistemaContabil.Infrastructure.Configuration;

public static class DatabaseConfiguration
{
    /// Strings de conexão para Oracle FIAP (múltiplas opções)
    public static readonly string[] ConnectionStrings = new[]
    {
        // Opção 1: Service Name FREEPDB1 (FORMATO CORRETO)
        "Data Source=//oracle.fiap.com.br:1521/orcl;User Id=rm559986; Password=240200;",
        
        // Opção 2: Service Name FREEPDB1 com timeout
        "Data Source=140.238.179.84:1521/FREEPDB1;User Id=appuser;Password=AppPass#2025;Connection Timeout=30;",
        
        // Opção 3: Formato TNS (se disponível)
        "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=140.238.179.84)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=FREEPDB1)));User Id=appuser;Password=AppPass#2025;",
        
        // Opção 4: Formato TNS com timeout
        "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=140.238.179.84)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=FREEPDB1)));User Id=appuser;Password=AppPass#2025;Connection Timeout=30;"
    };

    /// String de conexão atual (será testada)
    public static string ConnectionString => ConnectionStrings[0]; // Começa com a primeira opção

    public static IServiceCollection AddDatabaseConfiguration(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Configuração do DbContext
        services.AddDbContext<SistemaContabilDbContext>(options =>
        {
            options.UseOracle(ConnectionString, oracleOptions =>
            {
                oracleOptions.CommandTimeout(30);
            });
        });

        // Registro dos repositórios
        services.AddScoped<ICentroCustoRepository, CentroCustoRepository>();
        services.AddScoped<IContaRepository, ContaRepository>();
        services.AddScoped<IRegistroContabilRepository, RegistroContabilRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IVendasRepository, VendasRepository>();

        return services;
    }

    public static IServiceCollection AddDatabaseDevelopment(
        this IServiceCollection services)
    {
        services.AddDbContext<SistemaContabilDbContext>(options =>
        {
            options.UseOracle(ConnectionString, oracleOptions =>
            {
                oracleOptions.CommandTimeout(30);
            });
            
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        return services;
    }
}
