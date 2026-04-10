using SistemaContabil.Application.DTOs;

namespace SistemaContabil.Application.Services;

/// <summary>
/// Interface para serviço de aplicação de Vendas
/// </summary>
public interface IVendasAppService
{
    /// <summary>
    /// Obtém todas as vendas
    /// </summary>
    Task<IEnumerable<VendasDto>> ObterTodasAsync();

    /// <summary>
    /// Obtém venda por ID
    /// </summary>
    Task<VendasDto?> ObterPorIdAsync(int id);

    /// <summary>
    /// Cria uma nova venda
    /// </summary>
    Task<VendasDto> CriarAsync(CriarVendasDto dto);

    /// <summary>
    /// Atualiza uma venda existente
    /// </summary>
    Task<VendasDto> AtualizarAsync(int id, AtualizarVendasDto dto);

    /// <summary>
    /// Busca paginada de vendas
    /// </summary>
    Task<PagedResultDto<VendasDto>> SearchAsync(FiltroVendasDto filtro);
}
