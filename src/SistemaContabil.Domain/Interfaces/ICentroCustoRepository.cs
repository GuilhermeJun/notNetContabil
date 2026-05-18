using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Interfaces;

public interface ICentroCustoRepository : IRepository<CentroCusto>
{
    Task<CentroCusto?> GetByNomeAsync(string nome);

    Task<bool> ExistsByNomeAsync(string nome, int? excludeId = null);
    Task<IEnumerable<CentroCusto>> SearchByNomeAsync(string texto);
    Task<IEnumerable<CentroCusto>> GetWithRegistrosAsync();
    Task<CentroCusto?> GetWithRegistrosAsync(int id);
    Task<(IEnumerable<CentroCusto> Items, int TotalCount)> SearchPagedAsync(
        string? nome = null,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool isDescending = false);

    // Métodos adicionais para compatibilidade com serviços de domínio
    Task<CentroCusto?> ObterPorIdAsync(int id);
    Task<IEnumerable<CentroCusto>> ObterTodosAsync();
    Task<IEnumerable<CentroCusto>> BuscarPorNomeAsync(string nome);
    Task<CentroCusto> AdicionarAsync(CentroCusto entity);
    Task<CentroCusto> AtualizarAsync(CentroCusto entity);
    Task<bool> RemoverAsync(int id);
}
