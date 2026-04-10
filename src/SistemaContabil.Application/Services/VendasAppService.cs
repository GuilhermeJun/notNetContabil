using AutoMapper;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Domain.Interfaces;
using SistemaContabil.Domain.Services;

namespace SistemaContabil.Application.Services;

/// <summary>
/// Serviço de aplicação para Vendas
/// </summary>
public class VendasAppService : IVendasAppService
{
    private readonly IVendasService _vendasService;
    private readonly IVendasRepository _repository;
    private readonly IMapper _mapper;

    public VendasAppService(IVendasService vendasService, IVendasRepository repository, IMapper mapper)
    {
        _vendasService = vendasService;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VendasDto>> ObterTodasAsync()
    {
        var vendas = await _vendasService.ObterTodasAsync();
        return _mapper.Map<IEnumerable<VendasDto>>(vendas);
    }

    public async Task<VendasDto?> ObterPorIdAsync(int id)
    {
        var venda = await _vendasService.ObterPorIdAsync(id);
        return _mapper.Map<VendasDto?>(venda);
    }

    public async Task<VendasDto> CriarAsync(CriarVendasDto dto)
    {
        var venda = await _vendasService.CriarAsync(
            dto.ClienteIdCliente,
            dto.RegContIdRegCont,
            dto.VendaEventoIdEvento);
        return _mapper.Map<VendasDto>(venda);
    }

    public async Task<VendasDto> AtualizarAsync(int id, AtualizarVendasDto dto)
    {
        var venda = await _vendasService.AtualizarAsync(
            id,
            dto.ClienteIdCliente,
            dto.RegContIdRegCont,
            dto.VendaEventoIdEvento);
        return _mapper.Map<VendasDto>(venda);
    }

    public async Task<PagedResultDto<VendasDto>> SearchAsync(FiltroVendasDto filtro)
    {
        filtro.Validate();

        var (items, totalCount) = await _repository.SearchPagedAsync(
            clienteId: filtro.ClienteId,
            regContId: filtro.RegContId,
            vendaEventoId: filtro.VendaEventoId,
            page: filtro.Page,
            pageSize: filtro.PageSize,
            sortBy: filtro.SortBy,
            isDescending: filtro.IsDescending);

        var dtos = _mapper.Map<IEnumerable<VendasDto>>(items);

        return new PagedResultDto<VendasDto>
        {
            Items = dtos,
            Page = filtro.Page,
            PageSize = filtro.PageSize,
            TotalCount = totalCount
        };
    }
}
