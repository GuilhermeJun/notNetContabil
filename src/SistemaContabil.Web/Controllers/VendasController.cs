using Microsoft.AspNetCore.Mvc;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Application.Services;
using SistemaContabil.Web.Helpers;

namespace SistemaContabil.Web.Controllers;

/// <summary>
/// Controller para gerenciamento de Vendas
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class VendasController : ControllerBase
{
    private readonly IVendasAppService _vendasService;
    private readonly ILogger<VendasController> _logger;

    public VendasController(IVendasAppService vendasService, ILogger<VendasController> logger)
    {
        _vendasService = vendasService;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todas as vendas
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VendasDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VendasDto>>> GetAll()
    {
        try
        {
            var vendas = await _vendasService.ObterTodasAsync();
            return Ok(vendas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter todas as vendas");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Obtém uma venda por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VendasDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VendasDto>> GetById(int id)
    {
        try
        {
            var venda = await _vendasService.ObterPorIdAsync(id);
            if (venda == null)
            {
                return NotFound(new { message = $"Venda com ID {id} não encontrada" });
            }

            return Ok(venda);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter venda {VendaId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Cria uma nova venda
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(VendasDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VendasDto>> Create([FromBody] CriarVendasDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var venda = await _vendasService.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = venda.IdVendas }, venda);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar venda");
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro ao criar venda: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar venda");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Atualiza uma venda existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(VendasDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VendasDto>> Update(int id, [FromBody] AtualizarVendasDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var venda = await _vendasService.AtualizarAsync(id, dto);
            return Ok(venda);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar venda {VendaId}", id);
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Venda não encontrada: {VendaId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar venda {VendaId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }
}
