using SistemaContabil.Domain.Entities;
using SistemaContabil.Domain.Interfaces;

namespace SistemaContabil.Domain.Services;

/// Serviço de domínio para Registro Contábil
public class RegistroContabilService : IRegistroContabilService
{
    private readonly IRegistroContabilRepository _repository;
    private readonly IContaRepository _contaRepository;
    private readonly ICentroCustoRepository _centroCustoRepository;

    public RegistroContabilService(
        IRegistroContabilRepository repository,
        IContaRepository contaRepository,
        ICentroCustoRepository centroCustoRepository)
    {
        _repository = repository;
        _contaRepository = contaRepository;
        _centroCustoRepository = centroCustoRepository;
    }

    public async Task<RegistroContabil> CriarAsync(decimal valor, int contaId, int centroCustoId)
    {
        if (valor <= 0)
            throw new ArgumentException("Valor deve ser maior que zero", nameof(valor));

        // Verificar se a conta existe
        var conta = await _contaRepository.ObterPorIdAsync(contaId);
        if (conta == null)
            throw new ArgumentException($"Conta com ID {contaId} não encontrada", nameof(contaId));

        // Verificar se o centro de custo existe
        var centroCusto = await _centroCustoRepository.ObterPorIdAsync(centroCustoId);
        if (centroCusto == null)
            throw new ArgumentException($"Centro de custo com ID {centroCustoId} não encontrado", nameof(centroCustoId));

        var registro = new RegistroContabil
        {
            Valor = valor,
            ContaIdConta = contaId,
            CentroCustoId = centroCustoId
        };

        return await _repository.AdicionarAsync(registro);
    }

    public async Task<RegistroContabil> AtualizarAsync(int id, decimal valor, int contaId, int centroCustoId)
    {
        if (valor <= 0)
            throw new ArgumentException("Valor deve ser maior que zero", nameof(valor));

        var registro = await _repository.ObterPorIdAsync(id);
        if (registro == null)
            throw new InvalidOperationException($"Registro contábil com ID {id} não encontrado");

        // Verificar se a conta existe
        var conta = await _contaRepository.ObterPorIdAsync(contaId);
        if (conta == null)
            throw new ArgumentException($"Conta com ID {contaId} não encontrada", nameof(contaId));

        // Verificar se o centro de custo existe
        var centroCusto = await _centroCustoRepository.ObterPorIdAsync(centroCustoId);
        if (centroCusto == null)
            throw new ArgumentException($"Centro de custo com ID {centroCustoId} não encontrado", nameof(centroCustoId));

        registro.Valor = valor;
        registro.ContaIdConta = contaId;
        registro.CentroCustoId = centroCustoId;

        return await _repository.AtualizarAsync(registro);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var registro = await _repository.ObterPorIdAsync(id);
        if (registro == null)
            throw new InvalidOperationException($"Registro contábil com ID {id} não encontrado");

        return await _repository.RemoverAsync(id);
    }

    public async Task<RegistroContabil?> ObterPorIdAsync(int id)
    {
        return await _repository.ObterPorIdAsync(id);
    }

    public async Task<IEnumerable<RegistroContabil>> ObterTodosAsync()
    {
        return await _repository.ObterTodosAsync();
    }

    public async Task<IEnumerable<RegistroContabil>> ObterPorContaAsync(int contaId)
    {
        return await _repository.ObterPorContaAsync(contaId);
    }

    public async Task<IEnumerable<RegistroContabil>> ObterPorCentroCustoAsync(int centroCustoId)
    {
        return await _repository.ObterPorCentroCustoAsync(centroCustoId);
    }

    public async Task<IEnumerable<RegistroContabil>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        if (dataInicio > dataFim)
            throw new ArgumentException("Data de início deve ser anterior à data de fim");

        return await _repository.ObterPorPeriodoAsync(dataInicio, dataFim);
    }

    public async Task<IEnumerable<RegistroContabil>> ObterPorValorAsync(decimal valorMinimo, decimal valorMaximo)
    {
        if (valorMinimo < 0)
            throw new ArgumentException("Valor mínimo deve ser maior ou igual a zero");

        if (valorMinimo > valorMaximo)
            throw new ArgumentException("Valor mínimo deve ser menor ou igual ao valor máximo");

        return await _repository.ObterPorValorAsync(valorMinimo, valorMaximo);
    }

    public async Task<decimal> CalcularTotalPorContaAsync(int contaId)
    {
        var registros = await _repository.ObterPorContaAsync(contaId);
        return registros.Sum(r => r.Valor);
    }

    public async Task<decimal> CalcularTotalPorCentroCustoAsync(int centroCustoId)
    {
        var registros = await _repository.ObterPorCentroCustoAsync(centroCustoId);
        return registros.Sum(r => r.Valor);
    }

    public async Task<decimal> CalcularTotalPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        if (dataInicio > dataFim)
            throw new ArgumentException("Data de início deve ser anterior à data de fim");

        var registros = await _repository.ObterPorPeriodoAsync(dataInicio, dataFim);
        return registros.Sum(r => r.Valor);
    }

    public async Task<IEnumerable<RegistroContabil>> ObterComDetalhesAsync()
    {
        return await _repository.ObterComDetalhesAsync();
    }

    public async Task<RegistroContabil?> ObterComDetalhesAsync(int id)
    {
        return await _repository.ObterComDetalhesAsync(id);
    }
}
