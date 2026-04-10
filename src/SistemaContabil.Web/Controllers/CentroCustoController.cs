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

    /// Obtém todos os centros de custo
    /// <returns>Lista de centros de custo</returns>
    /// <response code="200">Retorna a lista de centros de custo</response>
    /// <response code="500">Erro interno do servidor</response>
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

    /// Obtém um centro de custo por ID
    /// <param name="id">ID do centro de custo</param>
    /// <returns>Centro de custo encontrado</returns>
    /// <response code="200">Retorna o centro de custo</response>
    /// <response code="404">Centro de custo não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
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

    /// Cria um novo centro de custo
    /// <param name="dto">Dados do centro de custo a ser criado</param>
    /// <returns>Centro de custo criado</returns>
    /// <response code="201">Centro de custo criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="500">Erro interno do servidor</response>
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

    /// Atualiza um centro de custo existente
    /// <param name="id">ID do centro de custo</param>
    /// <param name="dto">Dados atualizados do centro de custo</param>
    /// <returns>Centro de custo atualizado</returns>
    /// <response code="200">Centro de custo atualizado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Centro de custo não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
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

    /// Remove um centro de custo
    /// <param name="id">ID do centro de custo</param>
    /// <returns>Resultado da operação</returns>
    /// <response code="200">Centro de custo removido com sucesso</response>
    /// <response code="400">Centro de custo não pode ser removido (possui registros associados)</response>
    /// <response code="404">Centro de custo não encontrado</response>
    /// <response code="500">Erro interno do servidor</response>
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