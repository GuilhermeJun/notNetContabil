using Microsoft.AspNetCore.Mvc;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Application.Services;

namespace SistemaContabil.Web.Controllers.Mvc;

public class ContaController : Controller
{
    private readonly IContaAppService _service;
    private readonly ILogger<ContaController> _logger;

    public ContaController(IContaAppService service, ILogger<ContaController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: Conta
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Contas Contábeis";
        var contas = await _service.ObterTodasAsync();
        return View(contas);
    }

    // GET: Conta/Details/5
    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Detalhes da Conta Contábil";
        var conta = await _service.ObterPorIdAsync(id);
        if (conta == null)
        {
            TempData["Error"] = "Conta Contábil não encontrada.";
            return RedirectToAction(nameof(Index));
        }
        return View(conta);
    }

    // GET: Conta/Create
    public IActionResult Create()
    {
        ViewData["Title"] = "Nova Conta Contábil";
        return View();
    }

    // POST: Conta/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NomeContaContabil,Tipo")] CriarContaDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        try
        {
            await _service.CriarAsync(dto);
            TempData["Success"] = "Conta Contábil criada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar conta contábil");
            TempData["Error"] = $"Erro ao criar conta contábil: {ex.Message}";
            return View(dto);
        }
    }

    // GET: Conta/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Editar Conta Contábil";
        var conta = await _service.ObterPorIdAsync(id);
        if (conta == null)
        {
            TempData["Error"] = "Conta Contábil não encontrada.";
            return RedirectToAction(nameof(Index));
        }

        var dto = new AtualizarContaDto
        {
            NomeContaContabil = conta.NomeContaContabil,
            Tipo = conta.Tipo
        };

        return View(dto);
    }

    // POST: Conta/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("NomeContaContabil,Tipo")] AtualizarContaDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        try
        {
            await _service.AtualizarAsync(id, dto);
            TempData["Success"] = "Conta Contábil atualizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar conta contábil {Id}", id);
            TempData["Error"] = $"Erro ao atualizar conta contábil: {ex.Message}";
            return View(dto);
        }
    }

    // GET: Conta/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Excluir Conta Contábil";
        var conta = await _service.ObterPorIdAsync(id);
        if (conta == null)
        {
            TempData["Error"] = "Conta Contábil não encontrada.";
            return RedirectToAction(nameof(Index));
        }
        return View(conta);
    }

    // POST: Conta/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _service.RemoverAsync(id);
            TempData["Success"] = "Conta Contábil removida com sucesso!";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover conta contábil {Id}", id);
            TempData["Error"] = $"Erro ao remover conta contábil: {ex.Message}";
        }
        return RedirectToAction(nameof(Index));
    }
}
