using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Services;

public interface ICentroCustoService
{
    Task<CentroCusto> CriarAsync(string nome);
    Task<CentroCusto> AtualizarAsync(int id, string nome);
    Task<bool> RemoverAsync(int id);
    Task<CentroCusto?> ObterPorIdAsync(int id);
    Task<IEnumerable<CentroCusto>> ObterTodosAsync();
    Task<IEnumerable<CentroCusto>> BuscarPorNomeAsync(string nome);
    Task<bool> PodeSerRemovidoAsync(int id);
    Task<IEnumerable<CentroCusto>> ObterComRegistrosAsync();
}
