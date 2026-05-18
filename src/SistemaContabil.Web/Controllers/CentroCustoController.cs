using Microsoft.AspNetCore.Mvc;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Application.Services;
using SistemaContabil.Web.Helpers;

namespace SistemaContabil.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CentroCustoController : ControllerBase
{
    private readonly ICentroCustoAppService _centroCustoService;
    private readonly ILogger<CentroCustoController> _logger;

    public CentroCustoController(ICentroCustoAppService centroCustoService, ILogger<CentroCustoController> logger)
    {
        _centroCustoService = centroCustoService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CentroCustoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CentroCustoDto>>> ObterTodos()
    {
        try
        {
            var centrosCusto = await _centroCustoService.ObterTodosAsync();
            return Ok(centrosCusto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter centros de custo");
            return ApiControllerHelper.InternalServerErrorProblem(
                "Ocorreu um erro ao obter os centros de custo. Tente novamente mais tarde.",
                Request.Path);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CentroCustoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CentroCustoDto>> ObterPorId(int id)
    {
        try
        {
            if (id <= 0)
            {
                return ApiControllerHelper.BadRequestProblem(
                    "ID inválido. O ID deve ser maior que zero.",
                    Request.Path);
            }

            var centroCusto = await _centroCustoService.ObterPorIdAsync(id);
            if (centroCusto == null)
            {
                return ApiControllerHelper.NotFoundProblem(
                    $"Centro de custo com ID {id} não encontrado.",
                    Request.Path);
            }

            return Ok(centroCusto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter centro de custo {Id}", id);
            return ApiControllerHelper.InternalServerErrorProblem(
                "Ocorreu um erro ao obter o centro de custo. Tente novamente mais tarde.",
                Request.Path);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CentroCustoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CentroCustoDto>> Criar([FromBody] CriarCentroCustoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var problem = ApiControllerHelper.CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Erro de validação",
                    "Os dados fornecidos são inválidos.",
                    Request.Path,
                    new Dictionary<string, object?> { ["errors"] = ModelState });
                return new ObjectResult(problem) { StatusCode = StatusCodes.Status400BadRequest };
            }

            var centroCusto = await _centroCustoService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = centroCusto.IdCentroCusto }, centroCusto);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos para criação de centro de custo");
            return ApiControllerHelper.BadRequestProblem(ex.Message, Request.Path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar centro de custo");
            return ApiControllerHelper.InternalServerErrorProblem(
                "Ocorreu um erro ao criar o centro de custo. Tente novamente mais tarde.",
                Request.Path);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CentroCustoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CentroCustoDto>> Atualizar(int id, [FromBody] AtualizarCentroCustoDto dto)
    {
        try
        {
            if (id <= 0)
            {
                return ApiControllerHelper.BadRequestProblem(
                    "ID inválido. O ID deve ser maior que zero.",
                    Request.Path);
            }

            if (!ModelState.IsValid)
            {
                var problem = ApiControllerHelper.CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Erro de validação",
                    "Os dados fornecidos são inválidos.",
                    Request.Path,
                    new Dictionary<string, object?> { ["errors"] = ModelState });
                return new ObjectResult(problem) { StatusCode = StatusCodes.Status400BadRequest };
            }

            var centroCusto = await _centroCustoService.AtualizarAsync(id, dto);
            return Ok(centroCusto);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos para atualização de centro de custo {Id}", id);
            return ApiControllerHelper.BadRequestProblem(ex.Message, Request.Path);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Centro de custo {Id} não encontrado", id);
            return ApiControllerHelper.NotFoundProblem(ex.Message, Request.Path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar centro de custo {Id}", id);
            return ApiControllerHelper.InternalServerErrorProblem(
                "Ocorreu um erro ao atualizar o centro de custo. Tente novamente mais tarde.",
                Request.Path);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Remover(int id)
    {
        try
        {
            if (id <= 0)
            {
                return ApiControllerHelper.BadRequestProblem(
                    "ID inválido. O ID deve ser maior que zero.",
                    Request.Path);
            }

            var removido = await _centroCustoService.RemoverAsync(id);
            if (!removido)
            {
                return ApiControllerHelper.NotFoundProblem(
                    $"Centro de custo com ID {id} não encontrado.",
                    Request.Path);
            }

            return Ok(new { message = "Centro de custo removido com sucesso" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Centro de custo {Id} não pode ser removido", id);
            return ApiControllerHelper.BadRequestProblem(ex.Message, Request.Path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover centro de custo {Id}", id);
            return ApiControllerHelper.InternalServerErrorProblem(
                "Ocorreu um erro ao remover o centro de custo. Tente novamente mais tarde.",
                Request.Path);
        }
    }
}