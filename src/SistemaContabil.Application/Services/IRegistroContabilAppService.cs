using SistemaContabil.Application.DTOs;

namespace SistemaContabil.Application.Services;

public interface IRegistroContabilAppService
{
    Task<RegistroContabilDto> CriarAsync(CriarRegistroContabilDto dto);

    Task<RegistroContabilDto> AtualizarAsync(int id, AtualizarRegistroContabilDto dto);

    Task<bool> RemoverAsync(int id);

    Task<RegistroContabilDto?> ObterPorIdAsync(int id);

    Task<IEnumerable<RegistroContabilDto>> ObterTodosAsync();

    Task<IEnumerable<RegistroContabilDto>> ObterPorContaAsync(int contaId);

    Task<IEnumerable<RegistroContabilDto>> ObterPorCentroCustoAsync(int centroCustoId);

    Task<IEnumerable<RegistroContabilDto>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);

    Task<IEnumerable<RegistroContabilDto>> ObterPorValorAsync(decimal valorMinimo, decimal valorMaximo);

    Task<IEnumerable<RegistroContabilDto>> ObterComFiltrosAsync(FiltroRegistroContabilDto filtro);

    Task<RegistroContabilDetalhesDto?> ObterDetalhesAsync(int id);

    Task<decimal> CalcularTotalPorContaAsync(int contaId);

    Task<decimal> CalcularTotalPorCentroCustoAsync(int centroCustoId);

    Task<decimal> CalcularTotalPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);

    Task<PagedResultDto<RegistroContabilDto>> SearchAsync(FiltroRegistroContabilDto filtro);
}
