using AutoMapper;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Domain.Interfaces;
using SistemaContabil.Domain.Services;

namespace SistemaContabil.Application.Services;

/// Serviço de aplicação para Conta
public class ContaAppService : IContaAppService
{
    private readonly IContaService _contaService;
    private readonly IContaRepository _repository;
    private readonly IMapper _mapper;

    public ContaAppService(IContaService contaService, IContaRepository repository, IMapper mapper)
    {
        _contaService = contaService;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ContaDto>> ObterTodosAsync()
    {
        var contas = await _contaService.ObterTodasAsync();
        return _mapper.Map<IEnumerable<ContaDto>>(contas);
    }

    public async Task<ContaDto?> ObterPorIdAsync(int id)
    {
        var conta = await _contaService.ObterPorIdAsync(id);
        return _mapper.Map<ContaDto?>(conta);
    }

    public async Task<ContaDetalhesDto?> ObterDetalhesAsync(int id)
    {
        var conta = await _contaService.ObterPorIdAsync(id);
        return _mapper.Map<ContaDetalhesDto?>(conta);
    }

    public async Task<IEnumerable<ContaDto>> BuscarPorNomeAsync(string nome)
    {
        var contas = await _contaService.BuscarPorNomeAsync(nome);
        return _mapper.Map<IEnumerable<ContaDto>>(contas);
    }

    public async Task<IEnumerable<ContaDto>> BuscarPorTipoAsync(char tipo)
    {
        var contas = await _contaService.ObterPorTipoAsync(tipo);
        return _mapper.Map<IEnumerable<ContaDto>>(contas);
    }

    public async Task<ContaDto> CriarAsync(CriarContaDto dto)
    {
        var conta = await _contaService.CriarAsync(dto.NomeContaContabil, dto.Tipo);
        return _mapper.Map<ContaDto>(conta);
    }

    public async Task<ContaDto> AtualizarAsync(int id, AtualizarContaDto dto)
    {
        var conta = await _contaService.AtualizarAsync(id, dto.NomeContaContabil, dto.Tipo);
        return _mapper.Map<ContaDto>(conta);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await _contaService.RemoverAsync(id);
    }

    public async Task<bool> PodeSerRemovidaAsync(int id)
    {
        return await _contaService.PodeSerRemovidaAsync(id);
    }

    public async Task<IEnumerable<ContaDto>> ObterTodasAsync()
    {
        var contas = await _contaService.ObterTodasAsync();
        return _mapper.Map<IEnumerable<ContaDto>>(contas);
    }

    public async Task<IEnumerable<ContaDto>> ObterPorTipoAsync(char tipo)
    {
        var contas = await _contaService.ObterPorTipoAsync(tipo);
        return _mapper.Map<IEnumerable<ContaDto>>(contas);
    }

    public async Task<IEnumerable<ContaDto>> ObterContasReceitaAsync()
    {
        var contas = await _contaService.ObterContasReceitaAsync();
        return _mapper.Map<IEnumerable<ContaDto>>(contas);
    }

    public async Task<IEnumerable<ContaDto>> ObterContasDespesaAsync()
    {
        var contas = await _contaService.ObterContasDespesaAsync();
        return _mapper.Map<IEnumerable<ContaDto>>(contas);
    }

    public async Task<IEnumerable<ContaDto>> ObterComRegistrosAsync()
    {
        var contas = await _contaService.ObterComRegistrosAsync();
        return _mapper.Map<IEnumerable<ContaDto>>(contas);
    }

    public async Task<PagedResultDto<ContaDto>> SearchAsync(FiltroContaDto filtro)
    {
        filtro.Validate();

        var (items, totalCount) = await _repository.SearchPagedAsync(
            nome: filtro.Nome,
            tipo: filtro.Tipo,
            clienteId: filtro.ClienteId,
            page: filtro.Page,
            pageSize: filtro.PageSize,
            sortBy: filtro.SortBy,
            isDescending: filtro.IsDescending);

        var dtos = _mapper.Map<IEnumerable<ContaDto>>(items);

        return new PagedResultDto<ContaDto>
        {
            Items = dtos,
            Page = filtro.Page,
            PageSize = filtro.PageSize,
            TotalCount = totalCount
        };
    }
}
