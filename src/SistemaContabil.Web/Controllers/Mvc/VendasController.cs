using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Application.Services;

namespace SistemaContabil.Web.Controllers.Mvc;

public class VendasController : Controller
{
    private readonly IVendasAppService _service;
    private readonly IClienteAppService _clienteService;
    private readonly IRegistroContabilAppService _registroService;
    private readonly ILogger<VendasController> _logger;

    public VendasController(
        IVendasAppService service,
        IClienteAppService clienteService,
        IRegistroContabilAppService registroService,
        ILogger<VendasController> logger)
    {
        _service = service;
        _clienteService = clienteService;
        _registroService = registroService;
        _logger = logger;
    }

    private async Task<ViewDataDictionary> LoadSelectLists()
    {
        var clientes = await _clienteService.ObterTodosAsync();
        var registros = await _registroService.ObterTodosAsync();

        ViewData["ClienteIdCliente"] = new SelectList(clientes, "IdCliente", "NomeCliente");
        ViewData["RegContIdRegCont"] = new SelectList(registros, "IdRegCont", "IdRegCont");

        return ViewData;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Vendas";
        var vendas = await _service.ObterTodasAsync();
        return View(vendas);
    }

    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Detalhes da Venda";
        var venda = await _service.ObterPorIdAsync(id);
        if (venda == null)
        {
            TempData["Error"] = "Venda não encontrada.";
            return RedirectToAction(nameof(Index));
        }
        return View(venda);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Nova Venda";
        await LoadSelectLists();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ClienteIdCliente,RegContIdRegCont,VendaEventoIdEvento")] CriarVendasDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectLists();
            return View(dto);
        }

        try
        {
            await _service.CriarAsync(dto);
            TempData["Success"] = "Venda criada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar venda");
            TempData["Error"] = $"Erro ao criar venda: {ex.Message}";
            await LoadSelectLists();
            return View(dto);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Editar Venda";
        var venda = await _service.ObterPorIdAsync(id);
        if (venda == null)
        {
            TempData["Error"] = "Venda não encontrada.";
            return RedirectToAction(nameof(Index));
        }

        var dto = new AtualizarVendasDto
        {
            ClienteId = venda.ClienteId,
            RegContId = venda.RegContId,
            VendaEventoId = venda.VendaEventoId
        };

        await LoadSelectLists();
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ClienteIdCliente,RegContIdRegCont,VendaEventoIdEvento")] AtualizarVendasDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectLists();
            return View(dto);
        }

        try
        {
            await _service.AtualizarAsync(id, dto);
            TempData["Success"] = "Venda atualizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar venda {Id}", id);
            TempData["Error"] = $"Erro ao atualizar venda: {ex.Message}";
            await LoadSelectLists();
            return View(dto);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Excluir Venda";
        var venda = await _service.ObterPorIdAsync(id);
        if (venda == null)
        {
            TempData["Error"] = "Venda não encontrada.";
            return RedirectToAction(nameof(Index));
        }
        return View(venda);
    }

}
