using AutoMapper;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Domain.Interfaces;
using SistemaContabil.Domain.Services;

namespace SistemaContabil.Application.Services;

/// <summary>
/// Serviço de aplicação para Registro Contábil
/// </summary>
public class RegistroContabilAppService : IRegistroContabilAppService
{
    private readonly IRegistroContabilService _registroService;
    private readonly IRegistroContabilRepository _repository;
    private readonly IMapper _mapper;

    public RegistroContabilAppService(IRegistroContabilService registroService, IRegistroContabilRepository repository, IMapper mapper)
    {
        _registroService = registroService;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RegistroContabilDto>> ObterTodosAsync()
    {
        var registros = await _registroService.ObterTodosAsync();
        return _mapper.Map<IEnumerable<RegistroContabilDto>>(registros);
    }

    public async Task<RegistroContabilDto?> ObterPorIdAsync(int id)
    {
        var registro = await _registroService.ObterPorIdAsync(id);
        return _mapper.Map<RegistroContabilDto?>(registro);
    }

    public async Task<RegistroContabilDetalhesDto?> ObterDetalhesAsync(int id)
    {
        var registro = await _registroService.ObterComDetalhesAsync(id);
        return _mapper.Map<RegistroContabilDetalhesDto?>(registro);
    }

    public async Task<IEnumerable<RegistroContabilDto>> BuscarComFiltrosAsync(FiltroRegistroContabilDto filtro)
    {
        // Implementação simplificada - buscar todos e filtrar na aplicação
        var registros = await _registroService.ObterTodosAsync();
        
        // Aplicar filtros
        if (filtro.ContaId.HasValue)
            registros = registros.Where(r => r.ContaId == filtro.ContaId.Value);
            
        if (filtro.CentroCustoId.HasValue)
            registros = registros.Where(r => r.CentroCustoId == filtro.CentroCustoId.Value);
            
        if (filtro.ValorMin.HasValue)
            registros = registros.Where(r => r.Valor >= filtro.ValorMin.Value);
            
        if (filtro.ValorMax.HasValue)
            registros = registros.Where(r => r.Valor <= filtro.ValorMax.Value);
            
        return _mapper.Map<IEnumerable<RegistroContabilDto>>(registros);
    }

    public async Task<IEnumerable<RegistroContabilDto>> ObterPorContaAsync(int contaId)
    {
        var registros = await _registroService.ObterPorContaAsync(contaId);
        return _mapper.Map<IEnumerable<RegistroContabilDto>>(registros);
    }

    public async Task<IEnumerable<RegistroContabilDto>> ObterPorCentroCustoAsync(int centroCustoId)
    {
        var registros = await _registroService.ObterPorCentroCustoAsync(centroCustoId);
        return _mapper.Map<IEnumerable<RegistroContabilDto>>(registros);
    }

    public async Task<IEnumerable<RegistroContabilDto>> ObterPorValorAsync(decimal valorMinimo, decimal valorMaximo)
    {
        var registros = await _registroService.ObterPorValorAsync(valorMinimo, valorMaximo);
        return _mapper.Map<IEnumerable<RegistroContabilDto>>(registros);
    }

    public async Task<RegistroContabilDto> CriarAsync(CriarRegistroContabilDto dto)
    {
        var registro = await _registroService.CriarAsync(dto.Valor, dto.ContaId, dto.CentroCustoId);
        return _mapper.Map<RegistroContabilDto>(registro);
    }

    public async Task<RegistroContabilDto> AtualizarAsync(int id, AtualizarRegistroContabilDto dto)
    {
        var registro = await _registroService.AtualizarAsync(id, dto.Valor, dto.ContaId, dto.CentroCustoId);
        return _mapper.Map<RegistroContabilDto>(registro);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await _registroService.RemoverAsync(id);
    }

    public async Task<object> ObterEstatisticasAsync()
    {
        var registros = await _registroService.ObterTodosAsync();
        var valores = registros.Select(r => r.Valor).ToList();
        
        return new
        {
            TotalRegistros = valores.Count,
            ValorTotal = valores.Sum(),
            ValorMedio = valores.Count > 0 ? valores.Average() : 0,
            MaiorValor = valores.Count > 0 ? valores.Max() : 0,
            MenorValor = valores.Count > 0 ? valores.Min() : 0
        };
    }

    public async Task<IEnumerable<RegistroContabilDto>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        var registros = await _registroService.ObterPorPeriodoAsync(dataInicio, dataFim);
        return _mapper.Map<IEnumerable<RegistroContabilDto>>(registros);
    }

    public async Task<IEnumerable<RegistroContabilDto>> ObterComFiltrosAsync(FiltroRegistroContabilDto filtro)
    {
        // Implementação simplificada - buscar todos e filtrar na aplicação
        var registros = await _registroService.ObterTodosAsync();
        
        // Aplicar filtros
        if (filtro.ContaId.HasValue)
            registros = registros.Where(r => r.ContaId == filtro.ContaId.Value);
            
        if (filtro.CentroCustoId.HasValue)
            registros = registros.Where(r => r.CentroCustoId == filtro.CentroCustoId.Value);
            
        if (filtro.ValorMin.HasValue)
            registros = registros.Where(r => r.Valor >= filtro.ValorMin.Value);
            
        if (filtro.ValorMax.HasValue)
            registros = registros.Where(r => r.Valor <= filtro.ValorMax.Value);
            
        return _mapper.Map<IEnumerable<RegistroContabilDto>>(registros);
    }

    public async Task<decimal> CalcularTotalPorContaAsync(int contaId)
    {
        return await _registroService.CalcularTotalPorContaAsync(contaId);
    }

    public async Task<decimal> CalcularTotalPorCentroCustoAsync(int centroCustoId)
    {
        return await _registroService.CalcularTotalPorCentroCustoAsync(centroCustoId);
    }

    public async Task<decimal> CalcularTotalPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _registroService.CalcularTotalPorPeriodoAsync(dataInicio, dataFim);
    }

    public async Task<PagedResultDto<RegistroContabilDto>> SearchAsync(FiltroRegistroContabilDto filtro)
    {
        filtro.Validate();

        var (items, totalCount) = await _repository.SearchPagedAsync(
            valorMin: filtro.ValorMin,
            valorMax: filtro.ValorMax,
            contaId: filtro.ContaId,
            centroCustoId: filtro.CentroCustoId,
            dataInicio: filtro.DataInicio,
            dataFim: filtro.DataFim,
            page: filtro.Page,
            pageSize: filtro.PageSize,
            sortBy: filtro.SortBy,
            isDescending: filtro.IsDescending);

        var dtos = _mapper.Map<IEnumerable<RegistroContabilDto>>(items);

        return new PagedResultDto<RegistroContabilDto>
        {
            Items = dtos,
            Page = filtro.Page,
            PageSize = filtro.PageSize,
            TotalCount = totalCount
        };
    }
}
