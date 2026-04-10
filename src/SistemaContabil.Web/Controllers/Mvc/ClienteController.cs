using Microsoft.AspNetCore.Mvc;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Application.Services;

namespace SistemaContabil.Web.Controllers.Mvc;

public class ClienteController : Controller
{
    private readonly IClienteAppService _service;
    private readonly ILogger<ClienteController> _logger;

    public ClienteController(IClienteAppService service, ILogger<ClienteController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: Cliente
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Clientes";
        var clientes = await _service.ObterTodosAsync();
        return View(clientes);
    }

    // GET: Cliente/Details/5
    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Detalhes do Cliente";
        var cliente = await _service.ObterPorIdAsync(id);
        if (cliente == null)
        {
            TempData["Error"] = "Cliente não encontrado.";
            return RedirectToAction(nameof(Index));
        }
        return View(cliente);
    }

    // GET: Cliente/Create
    public IActionResult Create()
    {
        ViewData["Title"] = "Novo Cliente";
        return View();
    }

    // POST: Cliente/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NomeCliente,CpfCnpj,Email,Senha,Ativo")] CriarClienteDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        try
        {
            await _service.CriarAsync(dto);
            TempData["Success"] = "Cliente criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar cliente");
            TempData["Error"] = $"Erro ao criar cliente: {ex.Message}";
            return View(dto);
        }
    }

    // GET: Cliente/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Editar Cliente";
        var cliente = await _service.ObterPorIdAsync(id);
        if (cliente == null)
        {
            TempData["Error"] = "Cliente não encontrado.";
            return RedirectToAction(nameof(Index));
        }

        var dto = new AtualizarClienteDto
        {
            NomeCliente = cliente.NomeCliente,
            Email = cliente.Email,
            Ativo = cliente.Ativo
        };

        return View(dto);
    }

    // POST: Cliente/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("NomeCliente,Email,Ativo")] AtualizarClienteDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        try
        {
            await _service.AtualizarAsync(id, dto);
            TempData["Success"] = "Cliente atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar cliente {Id}", id);
            TempData["Error"] = $"Erro ao atualizar cliente: {ex.Message}";
            return View(dto);
        }
    }

    // GET: Cliente/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Excluir Cliente";
        var cliente = await _service.ObterPorIdAsync(id);
        if (cliente == null)
        {
            TempData["Error"] = "Cliente não encontrado.";
            return RedirectToAction(nameof(Index));
        }
        return View(cliente);
    }

    // POST: Cliente/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _service.RemoverAsync(id);
            TempData["Success"] = "Cliente removido com sucesso!";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover cliente {Id}", id);
            TempData["Error"] = $"Erro ao remover cliente: {ex.Message}";
        }
        return RedirectToAction(nameof(Index));
    }
}
