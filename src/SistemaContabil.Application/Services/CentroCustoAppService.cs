using AutoMapper;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Domain.Interfaces;
using SistemaContabil.Domain.Services;
using Xunit;

namespace SistemaContabil.Application.Services;

/// Serviço de aplicação para Centro de Custo
public class CentroCustoAppService : ICentroCustoAppService
{
    private readonly ICentroCustoService _centroCustoService;
    private readonly ICentroCustoRepository _repository;
    private readonly IMapper _mapper;

    public CentroCustoAppService(ICentroCustoService centroCustoService, ICentroCustoRepository repository, IMapper mapper)
    {
        _centroCustoService = centroCustoService;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CentroCustoDto>> ObterTodosAsync()
    {
        var centrosCusto = await _centroCustoService.ObterTodosAsync();
        return _mapper.Map<IEnumerable<CentroCustoDto>>(centrosCusto);
    }

    public async Task<CentroCustoDto?> ObterPorIdAsync(int id)
    {
        var centroCusto = await _centroCustoService.ObterPorIdAsync(id);
        return _mapper.Map<CentroCustoDto?>(centroCusto);
    }

    public async Task<CentroCustoDetalhesDto?> ObterDetalhesAsync(int id)
    {
        var centroCusto = await _centroCustoService.ObterPorIdAsync(id);
        return _mapper.Map<CentroCustoDetalhesDto?>(centroCusto);
    }

    public async Task<IEnumerable<CentroCustoDto>> BuscarPorNomeAsync(string nome)
    {
        var centrosCusto = await _centroCustoService.BuscarPorNomeAsync(nome);
        return _mapper.Map<IEnumerable<CentroCustoDto>>(centrosCusto);
    }

    public async Task<CentroCustoDto> CriarAsync(CriarCentroCustoDto dto)
    {
        var centroCusto = await _centroCustoService.CriarAsync(dto.NomeCentroCusto);
        return _mapper.Map<CentroCustoDto>(centroCusto);
    }

    public async Task<CentroCustoDto> AtualizarAsync(int id, AtualizarCentroCustoDto dto)
    {
        var centroCusto = await _centroCustoService.AtualizarAsync(id, dto.NomeCentroCusto);
        return _mapper.Map<CentroCustoDto>(centroCusto);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await _centroCustoService.RemoverAsync(id);
    }

    public async Task<bool> PodeSerRemovidoAsync(int id)
    {
        return await _centroCustoService.PodeSerRemovidoAsync(id);
    }

    public async Task<IEnumerable<CentroCustoDto>> ObterComRegistrosAsync()
    {
        var centrosCusto = await _centroCustoService.ObterComRegistrosAsync();
        return _mapper.Map<IEnumerable<CentroCustoDto>>(centrosCusto);
    }

    public async Task<PagedResultDto<CentroCustoDto>> SearchAsync(FiltroCentroCustoDto filtro)
    {
        filtro.Validate();

        var (items, totalCount) = await _repository.SearchPagedAsync(
            nome: filtro.Nome,
            page: filtro.Page,
            pageSize: filtro.PageSize,
            sortBy: filtro.SortBy,
            isDescending: filtro.IsDescending);

        var dtos = _mapper.Map<IEnumerable<CentroCustoDto>>(items);

        return new PagedResultDto<CentroCustoDto>
        {
            Items = dtos,
            Page = filtro.Page,
            PageSize = filtro.PageSize,
            TotalCount = totalCount
        };
    }
}
