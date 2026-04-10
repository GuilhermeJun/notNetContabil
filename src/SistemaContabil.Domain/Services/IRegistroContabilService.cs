using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Domain.Services;

/// Interface para serviços de Registro Contábil
public interface IRegistroContabilService
{
    /// Cria um novo registro contábil
    /// <param name="valor">Valor do registro</param>
    /// <param name="contaId">ID da conta</param>
    /// <param name="centroCustoId">ID do centro de custo</param>
    /// <returns>Registro contábil criado</returns>
    /// <exception cref="ArgumentException">Lançada quando dados são inválidos</exception>
    Task<RegistroContabil> CriarAsync(decimal valor, int contaId, int centroCustoId);

    /// Atualiza um registro contábil existente
    /// <param name="id">ID do registro</param>
    /// <param name="valor">Novo valor</param>
    /// <param name="contaId">ID da conta</param>
    /// <param name="centroCustoId">ID do centro de custo</param>
    /// <returns>Registro contábil atualizado</returns>
    /// <exception cref="ArgumentException">Lançada quando dados são inválidos</exception>
    /// <exception cref="InvalidOperationException">Lançada quando registro não é encontrado</exception>
    Task<RegistroContabil> AtualizarAsync(int id, decimal valor, int contaId, int centroCustoId);

    /// Remove um registro contábil
    /// <param name="id">ID do registro</param>
    /// <returns>True se removido com sucesso</returns>
    Task<bool> RemoverAsync(int id);

    /// Obtém um registro contábil por ID
    /// <param name="id">ID do registro</param>
    /// <returns>Registro contábil encontrado ou null</returns>
    Task<RegistroContabil?> ObterPorIdAsync(int id);

    /// Obtém todos os registros contábeis
    /// <returns>Lista de registros contábeis</returns>
    Task<IEnumerable<RegistroContabil>> ObterTodosAsync();

    /// Obtém registros contábeis por conta
    /// <param name="contaId">ID da conta</param>
    /// <returns>Lista de registros da conta</returns>
    Task<IEnumerable<RegistroContabil>> ObterPorContaAsync(int contaId);

    /// Obtém registros contábeis por centro de custo
    /// <param name="centroCustoId">ID do centro de custo</param>
    /// <returns>Lista de registros do centro de custo</returns>
    Task<IEnumerable<RegistroContabil>> ObterPorCentroCustoAsync(int centroCustoId);

    /// Obtém registros contábeis por período
    /// <param name="dataInicio">Data de início</param>
    /// <param name="dataFim">Data de fim</param>
    /// <returns>Lista de registros no período</returns>
    Task<IEnumerable<RegistroContabil>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);

    /// Obtém registros contábeis por valor
    /// <param name="valorMinimo">Valor mínimo</param>
    /// <param name="valorMaximo">Valor máximo</param>
    /// <returns>Lista de registros no intervalo de valores</returns>
    Task<IEnumerable<RegistroContabil>> ObterPorValorAsync(decimal valorMinimo, decimal valorMaximo);

    /// Calcula o total de registros por conta
    /// <param name="contaId">ID da conta</param>
    /// <returns>Valor total dos registros da conta</returns>
    Task<decimal> CalcularTotalPorContaAsync(int contaId);

    /// Calcula o total de registros por centro de custo
    /// <param name="centroCustoId">ID do centro de custo</param>
    /// <returns>Valor total dos registros do centro de custo</returns>
    Task<decimal> CalcularTotalPorCentroCustoAsync(int centroCustoId);

    /// Calcula o total de registros por período
    /// <param name="dataInicio">Data de início</param>
    /// <param name="dataFim">Data de fim</param>
    /// <returns>Valor total dos registros no período</returns>
    Task<decimal> CalcularTotalPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);

    /// Obtém registros contábeis com informações das entidades relacionadas
    /// <returns>Lista de registros com conta e centro de custo</returns>
    Task<IEnumerable<RegistroContabil>> ObterComDetalhesAsync();

    /// Obtém registro contábil com informações das entidades relacionadas
    /// <param name="id">ID do registro</param>
    /// <returns>Registro com conta e centro de custo ou null</returns>
    Task<RegistroContabil?> ObterComDetalhesAsync(int id);
}
