using Microsoft.AspNetCore.Mvc;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Application.Services;
using SistemaContabil.Web.Helpers;

namespace SistemaContabil.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClienteController : ControllerBase
{
    private readonly IClienteAppService _clienteService;
    private readonly ILogger<ClienteController> _logger;

    public ClienteController(IClienteAppService clienteService, ILogger<ClienteController> logger)
    {
        _clienteService = clienteService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClienteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll()
    {
        try
        {
            var clientes = await _clienteService.ObterTodosAsync();
            return Ok(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter todos os clientes");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteDto>> GetById(int id)
    {
        try
        {
            var cliente = await _clienteService.ObterPorIdAsync(id);
            if (cliente == null)
            {
                return NotFound(new { message = $"Cliente com ID {id} não encontrado" });
            }

            return Ok(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter cliente {ClienteId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClienteDto>> Create([FromBody] CriarClienteDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cliente = await _clienteService.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = cliente.IdCliente }, cliente);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar cliente");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar cliente");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClienteDto>> Update(int id, [FromBody] AtualizarClienteDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cliente = await _clienteService.AtualizarAsync(id, dto);
            return Ok(cliente);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar cliente {ClienteId}", id);
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cliente não encontrado: {ClienteId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar cliente {ClienteId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var sucesso = await _clienteService.RemoverAsync(id);
            if (!sucesso)
            {
                return NotFound(new { message = $"Cliente com ID {id} não encontrado" });
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro ao remover cliente {ClienteId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover cliente {ClienteId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }
}
