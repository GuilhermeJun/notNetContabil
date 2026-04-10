using Microsoft.AspNetCore.Mvc;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Application.Services;

namespace SistemaContabil.Web.Controllers.Mvc;

public class CentroCustoController : Controller
{
    private readonly ICentroCustoAppService _service;
    private readonly ILogger<CentroCustoController> _logger;

    public CentroCustoController(ICentroCustoAppService service, ILogger<CentroCustoController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: CentroCusto
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Centros de Custo";
        var centrosCusto = await _service.ObterTodosAsync();
        return View(centrosCusto);
    }

    // GET: CentroCusto/Details/5
    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Detalhes do Centro de Custo";
        var centroCusto = await _service.ObterPorIdAsync(id);
        if (centroCusto == null)
        {
            TempData["Error"] = "Centro de Custo não encontrado.";
            return RedirectToAction(nameof(Index));
        }
        return View(centroCusto);
    }

    // GET: CentroCusto/Create
    public IActionResult Create()
    {
        ViewData["Title"] = "Novo Centro de Custo";
        return View();
    }

    // POST: CentroCusto/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NomeCentroCusto")] CriarCentroCustoDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        try
        {
            await _service.CriarAsync(dto);
            TempData["Success"] = "Centro de Custo criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar centro de custo");
            TempData["Error"] = $"Erro ao criar centro de custo: {ex.Message}";
            return View(dto);
        }
    }

    // GET: CentroCusto/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Editar Centro de Custo";
        var centroCusto = await _service.ObterPorIdAsync(id);
        if (centroCusto == null)
        {
            TempData["Error"] = "Centro de Custo não encontrado.";
            return RedirectToAction(nameof(Index));
        }

        var dto = new AtualizarCentroCustoDto
        {
            NomeCentroCusto = centroCusto.NomeCentroCusto
        };

        return View(dto);
    }

    // POST: CentroCusto/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("NomeCentroCusto")] AtualizarCentroCustoDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        try
        {
            await _service.AtualizarAsync(id, dto);
            TempData["Success"] = "Centro de Custo atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar centro de custo {Id}", id);
            TempData["Error"] = $"Erro ao atualizar centro de custo: {ex.Message}";
            return View(dto);
        }
    }

    // GET: CentroCusto/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Excluir Centro de Custo";
        var centroCusto = await _service.ObterPorIdAsync(id);
        if (centroCusto == null)
        {
            TempData["Error"] = "Centro de Custo não encontrado.";
            return RedirectToAction(nameof(Index));
        }
        return View(centroCusto);
    }

    // POST: CentroCusto/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _service.RemoverAsync(id);
            TempData["Success"] = "Centro de Custo removido com sucesso!";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover centro de custo {Id}", id);
            TempData["Error"] = $"Erro ao remover centro de custo: {ex.Message}";
        }
        return RedirectToAction(nameof(Index));
    }
}
