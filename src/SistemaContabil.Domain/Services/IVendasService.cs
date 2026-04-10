using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Services;

/// Interface para serviços de Vendas
public interface IVendasService
{
    /// Cria uma nova venda
    Task<Vendas> CriarAsync(int clienteId, int regContId, long? vendaEventoId = null);

    /// Atualiza uma venda existente
    Task<Vendas> AtualizarAsync(int id, int clienteId, int regContId, long? vendaEventoId = null);

    /// Obtém venda por ID
    Task<Vendas?> ObterPorIdAsync(int id);

    /// Obtém todas as vendas
    Task<IEnumerable<Vendas>> ObterTodasAsync();

    /// Obtém vendas por cliente
    Task<IEnumerable<Vendas>> ObterPorClienteAsync(int clienteId);

    /// Obtém vendas por registro contábil
    Task<IEnumerable<Vendas>> ObterPorRegContAsync(int regContId);
}
