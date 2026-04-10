using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Application.Services;

namespace SistemaContabil.Web.Controllers.Mvc;

public class RegistroContabilController : Controller
{
    private readonly IRegistroContabilAppService _service;
    private readonly IContaAppService _contaService;
    private readonly ICentroCustoAppService _centroCustoService;
    private readonly ILogger<RegistroContabilController> _logger;

    public RegistroContabilController(
        IRegistroContabilAppService service,
        IContaAppService contaService,
        ICentroCustoAppService centroCustoService,
        ILogger<RegistroContabilController> logger)
    {
        _service = service;
        _contaService = contaService;
        _centroCustoService = centroCustoService;
        _logger = logger;
    }

    private async Task<ViewDataDictionary> LoadSelectLists()
    {
        var contas = await _contaService.ObterTodasAsync();
        var centrosCusto = await _centroCustoService.ObterTodosAsync();

        ViewData["ContaIdConta"] = new SelectList(contas, "IdContaContabil", "NomeContaContabil");
        ViewData["CentroCustoIdCentroCusto"] = new SelectList(centrosCusto, "IdCentroCusto", "NomeCentroCusto");

        return ViewData;
    }

    // GET: RegistroContabil
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Registros Contábeis";
        var registros = await _service.ObterTodosAsync();
        return View(registros);
    }

    // GET: RegistroContabil/Details/5
    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Detalhes do Registro Contábil";
        var registro = await _service.ObterPorIdAsync(id);
        if (registro == null)
        {
            TempData["Error"] = "Registro Contábil não encontrado.";
            return RedirectToAction(nameof(Index));
        }
        return View(registro);
    }

    // GET: RegistroContabil/Create
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Novo Registro Contábil";
        await LoadSelectLists();
        return View();
    }

    // POST: RegistroContabil/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Valor,ContaIdConta,CentroCustoIdCentroCusto")] CriarRegistroContabilDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectLists();
            return View(dto);
        }

        try
        {
            await _service.CriarAsync(dto);
            TempData["Success"] = "Registro Contábil criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar registro contábil");
            TempData["Error"] = $"Erro ao criar registro contábil: {ex.Message}";
            await LoadSelectLists();
            return View(dto);
        }
    }

    // GET: RegistroContabil/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Editar Registro Contábil";
        var registro = await _service.ObterPorIdAsync(id);
        if (registro == null)
        {
            TempData["Error"] = "Registro Contábil não encontrado.";
            return RedirectToAction(nameof(Index));
        }

        var dto = new AtualizarRegistroContabilDto
        {
            Valor = registro.Valor,
            ContaIdConta = registro.ContaIdConta,
            CentroCustoIdCentroCusto = registro.CentroCustoIdCentroCusto
        };

        await LoadSelectLists();
        return View(dto);
    }

    // POST: RegistroContabil/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Valor,ContaIdConta,CentroCustoIdCentroCusto")] AtualizarRegistroContabilDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectLists();
            return View(dto);
        }

        try
        {
            await _service.AtualizarAsync(id, dto);
            TempData["Success"] = "Registro Contábil atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar registro contábil {Id}", id);
            TempData["Error"] = $"Erro ao atualizar registro contábil: {ex.Message}";
            await LoadSelectLists();
            return View(dto);
        }
    }

    // GET: RegistroContabil/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Excluir Registro Contábil";
        var registro = await _service.ObterPorIdAsync(id);
        if (registro == null)
        {
            TempData["Error"] = "Registro Contábil não encontrado.";
            return RedirectToAction(nameof(Index));
        }
        return View(registro);
    }

    // POST: RegistroContabil/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _service.RemoverAsync(id);
            TempData["Success"] = "Registro Contábil removido com sucesso!";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover registro contábil {Id}", id);
            TempData["Error"] = $"Erro ao remover registro contábil: {ex.Message}";
        }
        return RedirectToAction(nameof(Index));
    }
}
