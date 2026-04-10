using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Services;

/// Interface para serviços de Cliente
public interface IClienteService
{
    /// Cria um novo cliente
    Task<Cliente> CriarAsync(string nome, string cpfCnpj, string email, string senha, char ativo = 'S');

    /// Atualiza um cliente existente
    Task<Cliente> AtualizarAsync(int id, string nome, string email, char ativo);

    /// Remove um cliente
    Task<bool> RemoverAsync(int id);

    /// Obtém cliente por ID
    Task<Cliente?> ObterPorIdAsync(int id);

    /// Obtém todos os clientes
    Task<IEnumerable<Cliente>> ObterTodosAsync();

    /// Busca clientes por nome
    Task<IEnumerable<Cliente>> BuscarPorNomeAsync(string nome);

    /// Obtém cliente por CPF/CNPJ
    Task<Cliente?> ObterPorCpfCnpjAsync(string cpfCnpj);

    /// Obtém cliente por email
    Task<Cliente?> ObterPorEmailAsync(string email);
}
