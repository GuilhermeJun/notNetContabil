using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Services;

public interface IContaService
{
    Task<Conta> CriarAsync(string nome, char tipo);
    Task<Conta> AtualizarAsync(int id, string nome, char tipo);
    Task<bool> RemoverAsync(int id);
    Task<Conta?> ObterPorIdAsync(int id);
    Task<IEnumerable<Conta>> ObterTodasAsync();
    Task<IEnumerable<Conta>> ObterPorTipoAsync(char tipo);
    Task<IEnumerable<Conta>> BuscarPorNomeAsync(string nome);
    Task<IEnumerable<Conta>> ObterContasReceitaAsync();
    Task<IEnumerable<Conta>> ObterContasDespesaAsync();
    Task<bool> PodeSerRemovidaAsync(int id);
    Task<IEnumerable<Conta>> ObterComRegistrosAsync();
}
