using Microsoft.AspNetCore.Mvc;
using Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Middleware.Models;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Middleware.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [SitemapInclude]
    public IActionResult Index()
    {
        return View();
    }

    [Route("custom-url")]
    [SitemapInclude]
    public IActionResult IndexWithCustomUrl()
    {
        return View(nameof(Index));
    }

    [SitemapExclude]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [ExcludeFromCodeCoverage]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}