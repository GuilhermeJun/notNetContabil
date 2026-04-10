using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Services;

/// Interface para serviços de Centro de Custo
public interface ICentroCustoService
{
    /// Cria um novo centro de custo
    /// <param name="nome">Nome do centro de custo</param>
    /// <returns>Centro de custo criado</returns>
    /// <exception cref="ArgumentException">Lançada quando o nome é inválido</exception>
    Task<CentroCusto> CriarAsync(string nome);

    /// Atualiza um centro de custo existente
    /// <param name="id">ID do centro de custo</param>
    /// <param name="nome">Novo nome</param>
    /// <returns>Centro de custo atualizado</returns>
    /// <exception cref="ArgumentException">Lançada quando dados são inválidos</exception>
    /// <exception cref="InvalidOperationException">Lançada quando centro de custo não é encontrado</exception>
    Task<CentroCusto> AtualizarAsync(int id, string nome);

    /// Remove um centro de custo
    /// <param name="id">ID do centro de custo</param>
    /// <returns>True se removido com sucesso</returns>
    /// <exception cref="InvalidOperationException">Lançada quando centro de custo não pode ser removido</exception>
    Task<bool> RemoverAsync(int id);

    /// Obtém um centro de custo por ID
    /// <param name="id">ID do centro de custo</param>
    /// <returns>Centro de custo encontrado ou null</returns>
    Task<CentroCusto?> ObterPorIdAsync(int id);

    /// Obtém todos os centros de custo
    /// <returns>Lista de centros de custo</returns>
    Task<IEnumerable<CentroCusto>> ObterTodosAsync();

    /// Busca centros de custo por nome
    /// <param name="nome">Nome para busca</param>
    /// <returns>Lista de centros de custo encontrados</returns>
    Task<IEnumerable<CentroCusto>> BuscarPorNomeAsync(string nome);

    /// Verifica se um centro de custo pode ser removido
    /// <param name="id">ID do centro de custo</param>
    /// <returns>True se pode ser removido</returns>
    Task<bool> PodeSerRemovidoAsync(int id);

    /// Obtém centros de custo com registros contábeis
    /// <returns>Lista de centros de custo com registros</returns>
    Task<IEnumerable<CentroCusto>> ObterComRegistrosAsync();
}
