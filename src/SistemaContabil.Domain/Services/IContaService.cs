using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Services;

/// Interface para serviços de Conta
public interface IContaService
{
    /// Cria uma nova conta
    /// <param name="nome">Nome da conta</param>
    /// <param name="tipo">Tipo da conta (R ou D)</param>
    /// <returns>Conta criada</returns>
    /// <exception cref="ArgumentException">Lançada quando dados são inválidos</exception>
    Task<Conta> CriarAsync(string nome, char tipo);

    /// Atualiza uma conta existente
    /// <param name="id">ID da conta</param>
    /// <param name="nome">Novo nome</param>
    /// <param name="tipo">Novo tipo</param>
    /// <returns>Conta atualizada</returns>
    /// <exception cref="ArgumentException">Lançada quando dados são inválidos</exception>
    /// <exception cref="InvalidOperationException">Lançada quando conta não é encontrada</exception>
    Task<Conta> AtualizarAsync(int id, string nome, char tipo);

    /// Remove uma conta
    /// <param name="id">ID da conta</param>
    /// <returns>True se removida com sucesso</returns>
    /// <exception cref="InvalidOperationException">Lançada quando conta não pode ser removida</exception>
    Task<bool> RemoverAsync(int id);

    /// Obtém uma conta por ID
    /// <param name="id">ID da conta</param>
    /// <returns>Conta encontrada ou null</returns>
    Task<Conta?> ObterPorIdAsync(int id);

    /// Obtém todas as contas
    /// <returns>Lista de contas</returns>
    Task<IEnumerable<Conta>> ObterTodasAsync();

    /// Obtém contas por tipo
    /// <param name="tipo">Tipo da conta (R ou D)</param>
    /// <returns>Lista de contas do tipo especificado</returns>
    Task<IEnumerable<Conta>> ObterPorTipoAsync(char tipo);

    /// Busca contas por nome
    /// <param name="nome">Nome para busca</param>
    /// <returns>Lista de contas encontradas</returns>
    Task<IEnumerable<Conta>> BuscarPorNomeAsync(string nome);

    /// Obtém contas de receita
    /// <returns>Lista de contas de receita</returns>
    Task<IEnumerable<Conta>> ObterContasReceitaAsync();

    /// Obtém contas de despesa
    /// <returns>Lista de contas de despesa</returns>
    Task<IEnumerable<Conta>> ObterContasDespesaAsync();

    /// Verifica se uma conta pode ser removida
    /// <param name="id">ID da conta</param>
    /// <returns>True se pode ser removida</returns>
    Task<bool> PodeSerRemovidaAsync(int id);

    /// Obtém contas com registros contábeis
    /// <returns>Lista de contas com registros</returns>
    Task<IEnumerable<Conta>> ObterComRegistrosAsync();
}
