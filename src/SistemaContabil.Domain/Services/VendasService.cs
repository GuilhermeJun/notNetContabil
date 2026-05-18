using SistemaContabil.Domain.Entities;
using SistemaContabil.Domain.Interfaces;

namespace SistemaContabil.Domain.Services;

public class VendasService : IVendasService
{
    private readonly IVendasRepository _repository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IRegistroContabilRepository _regContRepository;

    public VendasService(
        IVendasRepository repository,
        IClienteRepository clienteRepository,
        IRegistroContabilRepository regContRepository)
    {
        _repository = repository;
        _clienteRepository = clienteRepository;
        _regContRepository = regContRepository;
    }

    public async Task<Venda> CriarAsync(int clienteId, int regContId, long? vendaEventoId = null)
    {
        if (clienteId <= 0)
            throw new ArgumentException("ID do cliente deve ser maior que zero", nameof(clienteId));

        if (regContId <= 0)
            throw new ArgumentException("ID do registro contábil deve ser maior que zero", nameof(regContId));

        // Verificar se cliente existe
        var cliente = await _clienteRepository.ObterPorIdAsync(clienteId);
        if (cliente == null)
            throw new InvalidOperationException($"Cliente com ID {clienteId} não encontrado");

        // Verificar se registro contábil existe
        var regCont = await _regContRepository.ObterPorIdAsync(regContId);
        if (regCont == null)
            throw new InvalidOperationException($"Registro contábil com ID {regContId} não encontrado");

        var venda = new Venda
        {
            ClienteId = clienteId,
            RegContId = regContId,
            VendaEventoId = vendaEventoId
        };

        return await _repository.AdicionarAsync(venda);
    }

    public async Task<Venda> AtualizarAsync(int id, int clienteId, int regContId, long? vendaEventoId = null)
    {
        if (clienteId <= 0)
            throw new ArgumentException("ID do cliente deve ser maior que zero", nameof(clienteId));

        if (regContId <= 0)
            throw new ArgumentException("ID do registro contábil deve ser maior que zero", nameof(regContId));

        var venda = await _repository.ObterPorIdAsync(id);
        if (venda == null)
            throw new InvalidOperationException($"Venda com ID {id} não encontrada");

        // Verificar se cliente existe
        var cliente = await _clienteRepository.ObterPorIdAsync(clienteId);
        if (cliente == null)
            throw new InvalidOperationException($"Cliente com ID {clienteId} não encontrado");

        // Verificar se registro contábil existe
        var regCont = await _regContRepository.ObterPorIdAsync(regContId);
        if (regCont == null)
            throw new InvalidOperationException($"Registro contábil com ID {regContId} não encontrado");

        venda.ClienteId = clienteId;
        venda.RegContId = regContId;
        venda.VendaEventoId = vendaEventoId;

        return await _repository.AtualizarAsync(venda);
    }

    public async Task<Venda?> ObterPorIdAsync(int id)
    {
        return await _repository.ObterPorIdAsync(id);
    }

    public async Task<IEnumerable<Venda>> ObterTodasAsync()
    {
        return await _repository.ObterTodosAsync();
    }

    public async Task<IEnumerable<Venda>> ObterPorClienteAsync(int clienteId)
    {
        return await _repository.GetByClienteIdAsync(clienteId);
    }

    public async Task<IEnumerable<Venda>> ObterPorRegContAsync(int regContId)
    {
        return await _repository.GetByRegContIdAsync(regContId);
    }
}
