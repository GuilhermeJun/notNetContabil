using SistemaContabil.Application.DTOs;

namespace SistemaContabil.Application.Services;

public interface IVendasAppService
{
    Task<IEnumerable<VendasDto>> ObterTodasAsync();
    Task<VendasDto?> ObterPorIdAsync(int id);
    Task<VendasDto> CriarAsync(CriarVendasDto dto);
    Task<VendasDto> AtualizarAsync(int id, AtualizarVendasDto dto);
    Task<PagedResultDto<VendasDto>> SearchAsync(FiltroVendasDto filtro);
}
