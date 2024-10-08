using Microsoft.AspNetCore.Mvc;
using Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Middleware.Models;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Middleware.Controllers;

[ExcludeFromCodeCoverage]
public class HomeController : Controller
{
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
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}