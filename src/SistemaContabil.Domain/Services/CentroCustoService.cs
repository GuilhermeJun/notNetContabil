using SistemaContabil.Domain.Entities;
using SistemaContabil.Domain.Interfaces;

namespace SistemaContabil.Domain.Services;

/// Serviço de domínio para Centro de Custo
public class CentroCustoService : ICentroCustoService
{
    private readonly ICentroCustoRepository _repository;
    private readonly IRegistroContabilRepository _registroRepository;

    public CentroCustoService(ICentroCustoRepository repository, IRegistroContabilRepository registroRepository)
    {
        _repository = repository;
        _registroRepository = registroRepository;
    }

    public async Task<CentroCusto> CriarAsync(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do centro de custo é obrigatório", nameof(nome));

        if (nome.Length > 70)
            throw new ArgumentException("Nome do centro de custo deve ter no máximo 70 caracteres", nameof(nome));

        // Verificar se já existe um centro de custo com o mesmo nome
        var existente = await _repository.BuscarPorNomeAsync(nome);
        if (existente.Any())
            throw new ArgumentException("Já existe um centro de custo com este nome", nameof(nome));

        var centroCusto = new CentroCusto
        {
            NomeCentroCusto = nome.Trim()
        };

        return await _repository.AdicionarAsync(centroCusto);
    }

    public async Task<CentroCusto> AtualizarAsync(int id, string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do centro de custo é obrigatório", nameof(nome));

        if (nome.Length > 70)
            throw new ArgumentException("Nome do centro de custo deve ter no máximo 70 caracteres", nameof(nome));

        var centroCusto = await _repository.ObterPorIdAsync(id);
        if (centroCusto == null)
            throw new InvalidOperationException($"Centro de custo com ID {id} não encontrado");

        // Verificar se já existe outro centro de custo com o mesmo nome
        var existente = await _repository.BuscarPorNomeAsync(nome);
        if (existente.Any(c => c.IdCentroCusto != id))
            throw new ArgumentException("Já existe outro centro de custo com este nome", nameof(nome));

        centroCusto.NomeCentroCusto = nome.Trim();
        return await _repository.AtualizarAsync(centroCusto);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var centroCusto = await _repository.ObterPorIdAsync(id);
        if (centroCusto == null)
            throw new InvalidOperationException($"Centro de custo com ID {id} não encontrado");

        // Verificar se pode ser removido (não possui registros contábeis)
        var podeSerRemovido = await PodeSerRemovidoAsync(id);
        if (!podeSerRemovido)
            throw new InvalidOperationException("Centro de custo não pode ser removido pois possui registros contábeis");

        return await _repository.RemoverAsync(id);
    }

    public async Task<CentroCusto?> ObterPorIdAsync(int id)
    {
        return await _repository.ObterPorIdAsync(id);
    }

    public async Task<IEnumerable<CentroCusto>> ObterTodosAsync()
    {
        return await _repository.ObterTodosAsync();
    }

    public async Task<IEnumerable<CentroCusto>> BuscarPorNomeAsync(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Enumerable.Empty<CentroCusto>();

        return await _repository.BuscarPorNomeAsync(nome);
    }

    public async Task<bool> PodeSerRemovidoAsync(int id)
    {
        var registros = await _registroRepository.ObterPorCentroCustoAsync(id);
        return !registros.Any();
    }

    public async Task<IEnumerable<CentroCusto>> ObterComRegistrosAsync()
    {
        var todos = await _repository.ObterTodosAsync();
        var comRegistros = new List<CentroCusto>();

        foreach (var centro in todos)
        {
            var registros = await _registroRepository.ObterPorCentroCustoAsync(centro.IdCentroCusto);
            if (registros.Any())
            {
                comRegistros.Add(centro);
            }
        }

        return comRegistros;
    }
}
