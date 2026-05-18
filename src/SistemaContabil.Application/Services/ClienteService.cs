using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Application.Services;

public class ClienteService
{
    private readonly ClienteService _clienteService;
    private readonly ClienteRepository _repository;
    private readonly IMapper _mapper;

    public ClienteService(ClienteService clienteService, ClienteRepository repository, IMapper mapper)
    {
        _clienteService = clienteService;
        _repository = repository;
        _mapper = mapper;
    }

    static async Task<IResult> ObterTodosAsync(SistemaContabilDbContext db)
    {
        return TypedResults.Ok(await db.Todos.Select(x => new ClienteDto(x)).ToArrayAsync());
    }
    /*
    public async Task<IEnumerable<ClienteDto>> ObterTodosAsync()
    {
        var clientes = await _clienteService.ObterTodosAsync();
        return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
    }
    */
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
}
