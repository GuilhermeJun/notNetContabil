using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Interfaces;

public interface IVendasRepository : IRepository<Venda>
{
    Task<IEnumerable<Venda>> GetByClienteIdAsync(int clienteId);

    Task<IEnumerable<Venda>> GetByRegContIdAsync(int regContId);

    Task<(IEnumerable<Venda> Items, int TotalCount)> SearchPagedAsync(
        int? clienteId = null,
        int? regContId = null,
        long? vendaEventoId = null,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool isDescending = false);

    // Métodos de compatibilidade
    Task<Venda?> ObterPorIdAsync(int id);
    Task<IEnumerable<Venda>> ObterTodosAsync();
    Task<Venda> AdicionarAsync(Venda entity);
    Task<Venda> AtualizarAsync(Venda entity);
    Task<bool> RemoverAsync(int id);
}
