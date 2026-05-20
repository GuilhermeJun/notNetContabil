using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Services;

public interface IClienteService
{
    Task<Cliente> CriarAsync(string nome, string cpf, string email, char ativo = 'S');
    Task<Cliente> AtualizarAsync(int id, string nome, string email, char ativo);
    Task<bool> RemoverAsync(int id);
    Task<Cliente?> ObterPorIdAsync(int id);
    Task<IEnumerable<Cliente>> ObterTodosAsync();
    Task<IEnumerable<Cliente>> BuscarPorNomeAsync(string nome);
    Task<Cliente?> ObterPorCpfAsync(string cpf);
    Task<Cliente?> ObterPorEmailAsync(string email);
}
