using SistemaContabil.Application.DTOs;

namespace SistemaContabil.Application.Services;

public interface IClienteAppService
{
    Task<IEnumerable<ClienteDto>> ObterTodosAsync();

    Task<ClienteDto?> ObterPorIdAsync(int id);

    Task<ClienteDto> CriarAsync(CriarClienteDto dto);

    Task<ClienteDto> AtualizarAsync(int id, AtualizarClienteDto dto);

    Task<bool> RemoverAsync(int id);
}
