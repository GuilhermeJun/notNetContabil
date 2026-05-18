using Microsoft.AspNetCore.Mvc;

namespace SistemaContabil.Web.Controllers.Mvc;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Início - Sistema Contábil";
        return View();
    }
}
