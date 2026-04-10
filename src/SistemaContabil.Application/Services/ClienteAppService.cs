using AutoMapper;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Domain.Interfaces;
using SistemaContabil.Domain.Services;

namespace SistemaContabil.Application.Services;

/// Serviço de aplicação para Cliente
public class ClienteAppService : IClienteAppService
{
    private readonly IClienteService _clienteService;
    private readonly IClienteRepository _repository;
    private readonly IMapper _mapper;

    public ClienteAppService(IClienteService clienteService, IClienteRepository repository, IMapper mapper)
    {
        _clienteService = clienteService;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClienteDto>> ObterTodosAsync()
    {
        var clientes = await _clienteService.ObterTodosAsync();
        return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
    }

    public async Task<ClienteDto?> ObterPorIdAsync(int id)
    {
        var cliente = await _clienteService.ObterPorIdAsync(id);
        return _mapper.Map<ClienteDto?>(cliente);
    }

    public async Task<ClienteDto> CriarAsync(CriarClienteDto dto)
    {
        var ativoChar = !string.IsNullOrEmpty(dto.Ativo) ? dto.Ativo[0] : 'S';
        var cliente = await _clienteService.CriarAsync(
            dto.NomeCliente,
            dto.CpfCnpj,
            dto.Email,
            dto.Senha,
            ativoChar);
        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<ClienteDto> AtualizarAsync(int id, AtualizarClienteDto dto)
    {
        var ativoChar = !string.IsNullOrEmpty(dto.Ativo) ? dto.Ativo[0] : 'S';
        var cliente = await _clienteService.AtualizarAsync(id, dto.NomeCliente, dto.Email, ativoChar);
        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await _clienteService.RemoverAsync(id);
    }

    public async Task<PagedResultDto<ClienteDto>> SearchAsync(FiltroClienteDto filtro)
    {
        filtro.Validate();

        var (items, totalCount) = await _repository.SearchPagedAsync(
            nome: filtro.Nome,
            cpfCnpj: filtro.CpfCnpj,
            ativo: filtro.Ativo,
            email: filtro.Email,
            page: filtro.Page,
            pageSize: filtro.PageSize,
            sortBy: filtro.SortBy,
            isDescending: filtro.IsDescending);

        var dtos = _mapper.Map<IEnumerable<ClienteDto>>(items);

        return new PagedResultDto<ClienteDto>
        {
            Items = dtos,
            Page = filtro.Page,
            PageSize = filtro.PageSize,
            TotalCount = totalCount
        };
    }
}
