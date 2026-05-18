using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Interfaces;
public interface IContaRepository : IRepository<Conta>
{
    Task<Conta?> GetByNomeAsync(string nome);
    Task<bool> ExistsByNomeAsync(string nome, int? excludeId = null);
    Task<IEnumerable<Conta>> GetByTipoAsync(char tipo);
    Task<IEnumerable<Conta>> SearchByNomeAsync(string texto);
    Task<IEnumerable<Conta>> GetWithRegistrosAsync();
    Task<Conta?> GetWithRegistrosAsync(int id);
    Task<IEnumerable<Conta>> GetContasReceitaAsync();
    Task<IEnumerable<Conta>> GetContasDespesaAsync();
    Task<(IEnumerable<Conta> Items, int TotalCount)> SearchPagedAsync(
        string? nome = null,
        char? tipo = null,
        int? clienteId = null,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool isDescending = false);

    // Métodos adicionais para compatibilidade com serviços de domínio
    Task<Conta?> ObterPorIdAsync(int id);
    Task<IEnumerable<Conta>> ObterTodosAsync();
    Task<IEnumerable<Conta>> BuscarPorNomeAsync(string nome);
    Task<Conta> AdicionarAsync(Conta entity);
    Task<Conta> AtualizarAsync(Conta entity);
    Task<bool> RemoverAsync(int id);
    Task<IEnumerable<Conta>> ObterPorTipoAsync(char tipo);
    Task<IEnumerable<Conta>> ObterContasReceitaAsync();
    Task<IEnumerable<Conta>> ObterContasDespesaAsync();
    Task<IEnumerable<Conta>> ObterComRegistrosAsync();
}
