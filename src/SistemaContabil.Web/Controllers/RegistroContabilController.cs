using Microsoft.AspNetCore.Mvc;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Application.Services;

namespace SistemaContabil.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistroContabilController : ControllerBase
{
    private readonly IRegistroContabilAppService _registroService;
    private readonly ILogger<RegistroContabilController> _logger;

    public RegistroContabilController(IRegistroContabilAppService registroService, ILogger<RegistroContabilController> logger)
    {
        _registroService = registroService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegistroContabilDto>>> ObterTodos()
    {
        try
        {
            var registros = await _registroService.ObterTodosAsync();
            return Ok(registros);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter registros contábeis");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RegistroContabilDto>> ObterPorId(int id)
    {
        try
        {
            var registro = await _registroService.ObterPorIdAsync(id);
            if (registro == null)
                return NotFound($"Registro contábil com ID {id} não encontrado");

            return Ok(registro);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter registro contábil {Id}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<RegistroContabilDto>> Criar([FromBody] CriarRegistroContabilDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registro = await _registroService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = registro.IdRegCont }, registro);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos para criação de registro contábil");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar registro contábil");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RegistroContabilDto>> Atualizar(int id, [FromBody] AtualizarRegistroContabilDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registro = await _registroService.AtualizarAsync(id, dto);
            return Ok(registro);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos para atualização de registro contábil {Id}", id);
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Registro contábil {Id} não encontrado", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar registro contábil {Id}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Remover(int id)
    {
        try
        {
            var removido = await _registroService.RemoverAsync(id);
            if (!removido)
                return NotFound($"Registro contábil com ID {id} não encontrado");

            return Ok(new { message = "Registro contábil removido com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover registro contábil {Id}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }
}