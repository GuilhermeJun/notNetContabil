using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Services;

public interface IVendasService
{
    Task<Venda> CriarAsync(int clienteId, int regContId, long? vendaEventoId = null);
    Task<Venda> AtualizarAsync(int id, int clienteId, int regContId, long? vendaEventoId = null);
    Task<Venda?> ObterPorIdAsync(int id);
    Task<IEnumerable<Venda>> ObterTodasAsync();
    Task<IEnumerable<Venda>> ObterPorClienteAsync(int clienteId);
    Task<IEnumerable<Venda>> ObterPorRegContAsync(int regContId);
}
