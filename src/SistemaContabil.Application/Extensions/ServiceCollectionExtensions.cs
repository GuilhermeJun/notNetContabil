using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SistemaContabil.Application.Mappings;
using SistemaContabil.Application.Services;
using SistemaContabil.Application.Validators;
using SistemaContabil.Domain.Services;

namespace SistemaContabil.Application.Extensions;

/// Extensões para configuração de serviços da camada de aplicação
public static class ServiceCollectionExtensions
{
    /// Adiciona serviços da camada de aplicação
    /// <param name="services">Coleção de serviços</param>
    /// <returns>Coleção de serviços configurada</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Configuração do AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // Configuração do FluentValidation
        services.AddValidatorsFromAssemblyContaining<CentroCustoValidator>();

        // Registro dos serviços de aplicação
        services.AddScoped<ICentroCustoAppService, CentroCustoAppService>();
        services.AddScoped<IContaAppService, ContaAppService>();
        services.AddScoped<IRegistroContabilAppService, RegistroContabilAppService>();
        services.AddScoped<IClienteAppService, ClienteAppService>();
        services.AddScoped<IVendasAppService, VendasAppService>();

        // Registro dos serviços de domínio
        // (registrations moved to infrastructure layer to avoid cross-layer service registrations)

        return services;
    }
}
