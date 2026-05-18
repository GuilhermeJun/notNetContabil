using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Interfaces;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<Cliente?> GetByCpfCnpjAsync(string cpfCnpj);
    Task<Cliente?> GetByEmailAsync(string email);
    Task<bool> ExistsByCpfCnpjAsync(string cpfCnpj, int? excludeId = null);
    Task<bool> ExistsByEmailAsync(string email, int? excludeId = null);
    Task<IEnumerable<Cliente>> SearchByNomeAsync(string nome);
    Task<(IEnumerable<Cliente> Items, int TotalCount)> SearchPagedAsync(
        string? nome = null,
        string? cpfCnpj = null,
        char? ativo = null,
        string? email = null,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool isDescending = false);

    // Métodos de compatibilidade
    Task<Cliente?> ObterPorIdAsync(int id);
    Task<IEnumerable<Cliente>> ObterTodosAsync();
    Task<Cliente> AdicionarAsync(Cliente entity);
    Task<Cliente> AtualizarAsync(Cliente entity);
    Task<bool> RemoverAsync(int id);
}
