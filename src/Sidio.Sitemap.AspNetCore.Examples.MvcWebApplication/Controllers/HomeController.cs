using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Models;

namespace Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

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