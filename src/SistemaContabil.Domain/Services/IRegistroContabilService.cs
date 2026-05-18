using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Services;

public interface IRegistroContabilService
{
    Task<RegistroContabil> CriarAsync(decimal valor, int contaId, int centroCustoId);
    Task<RegistroContabil> AtualizarAsync(int id, decimal valor, int contaId, int centroCustoId);
    Task<bool> RemoverAsync(int id);
    Task<RegistroContabil?> ObterPorIdAsync(int id);
    Task<IEnumerable<RegistroContabil>> ObterTodosAsync();
    Task<IEnumerable<RegistroContabil>> ObterPorContaAsync(int contaId);
    Task<IEnumerable<RegistroContabil>> ObterPorCentroCustoAsync(int centroCustoId);
    Task<IEnumerable<RegistroContabil>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<RegistroContabil>> ObterPorValorAsync(decimal valorMinimo, decimal valorMaximo);
    Task<decimal> CalcularTotalPorContaAsync(int contaId);
    Task<decimal> CalcularTotalPorCentroCustoAsync(int centroCustoId);
    Task<decimal> CalcularTotalPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<RegistroContabil>> ObterComDetalhesAsync();
    Task<RegistroContabil?> ObterComDetalhesAsync(int id);
}
