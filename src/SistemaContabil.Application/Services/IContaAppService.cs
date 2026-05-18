using SistemaContabil.Application.DTOs;

namespace SistemaContabil.Application.Services;

public interface IContaAppService
{
    Task<ContaDto> CriarAsync(CriarContaDto dto);
    Task<ContaDto> AtualizarAsync(int id, AtualizarContaDto dto);
    Task<bool> RemoverAsync(int id);
    Task<ContaDto?> ObterPorIdAsync(int id);
    Task<IEnumerable<ContaDto>> ObterTodasAsync();
    Task<IEnumerable<ContaDto>> ObterPorTipoAsync(char tipo);
    Task<IEnumerable<ContaDto>> BuscarPorNomeAsync(string nome);
    Task<ContaDetalhesDto?> ObterDetalhesAsync(int id);
    Task<IEnumerable<ContaDto>> ObterContasReceitaAsync();
    Task<IEnumerable<ContaDto>> ObterContasDespesaAsync();
    Task<PagedResultDto<ContaDto>> SearchAsync(FiltroContaDto filtro);
    Task<IEnumerable<ContaDto>> ObterComRegistrosAsync();
    Task<bool> PodeSerRemovidaAsync(int id);
}