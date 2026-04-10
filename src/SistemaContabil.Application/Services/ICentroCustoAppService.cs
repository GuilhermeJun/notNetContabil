using SistemaContabil.Application.DTOs;

namespace SistemaContabil.Application.Services;

public interface ICentroCustoAppService
{
    Task<CentroCustoDto> CriarAsync(CriarCentroCustoDto dto);

    Task<CentroCustoDto> AtualizarAsync(int id, AtualizarCentroCustoDto dto);

    Task<bool> RemoverAsync(int id);

    Task<CentroCustoDto?> ObterPorIdAsync(int id);

    Task<IEnumerable<CentroCustoDto>> ObterTodosAsync();

    Task<IEnumerable<CentroCustoDto>> BuscarPorNomeAsync(string nome);

    Task<CentroCustoDetalhesDto?> ObterDetalhesAsync(int id);

    Task<IEnumerable<CentroCustoDto>> ObterComRegistrosAsync();

    Task<bool> PodeSerRemovidoAsync(int id);

    Task<PagedResultDto<CentroCustoDto>> SearchAsync(FiltroCentroCustoDto filtro);
}
