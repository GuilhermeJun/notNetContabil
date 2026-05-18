using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Interfaces;

public interface IRegistroContabilRepository : IRepository<RegistroContabil>
{
    Task<IEnumerable<RegistroContabil>> GetByContaAsync(int contaId);
    Task<IEnumerable<RegistroContabil>> GetByCentroCustoAsync(int centroCustoId);
    Task<IEnumerable<RegistroContabil>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<RegistroContabil>> GetByValorAsync(decimal valorMinimo, decimal valorMaximo);
    Task<IEnumerable<RegistroContabil>> GetWithDetailsAsync();
    Task<RegistroContabil?> GetWithDetailsAsync(int id);
    Task<decimal> GetTotalByContaAsync(int contaId);
    Task<decimal> GetTotalByCentroCustoAsync(int centroCustoId);
    Task<decimal> GetTotalByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<RegistroContabil>> GetOrderedByDataAsync(bool ascending = true);
    Task<IEnumerable<RegistroContabil>> GetOrderedByValorAsync(bool ascending = true);
    Task<(IEnumerable<RegistroContabil> Items, int TotalCount)> SearchPagedAsync(
        decimal? valorMin = null,
        decimal? valorMax = null,
        int? contaId = null,
        int? centroCustoId = null,
        DateTime? dataInicio = null,
        DateTime? dataFim = null,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool isDescending = false);

    // Métodos adicionais para compatibilidade com serviços de domínio
    Task<RegistroContabil?> ObterPorIdAsync(int id);
    Task<IEnumerable<RegistroContabil>> ObterTodosAsync();
    Task<RegistroContabil> AdicionarAsync(RegistroContabil entity);
    Task<RegistroContabil> AtualizarAsync(RegistroContabil entity);
    Task<bool> RemoverAsync(int id);
    Task<IEnumerable<RegistroContabil>> ObterPorContaAsync(int contaId);
    Task<IEnumerable<RegistroContabil>> ObterPorCentroCustoAsync(int centroCustoId);
    Task<IEnumerable<RegistroContabil>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<RegistroContabil>> ObterPorValorAsync(decimal valorMinimo, decimal valorMaximo);
    Task<IEnumerable<RegistroContabil>> ObterComDetalhesAsync();
    Task<RegistroContabil?> ObterComDetalhesAsync(int id);
    Task<decimal> CalcularTotalPorContaAsync(int contaId);
    Task<decimal> CalcularTotalPorCentroCustoAsync(int centroCustoId);
    Task<decimal> CalcularTotalPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
}
