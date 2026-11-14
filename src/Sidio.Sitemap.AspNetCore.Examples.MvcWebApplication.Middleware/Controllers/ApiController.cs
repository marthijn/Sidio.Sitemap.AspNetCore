using Microsoft.AspNetCore.Mvc;

namespace Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Middleware.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public sealed class ApiController : Controller
{
    [HttpGet]
    [SitemapInclude]
    public IActionResult Included()
    {
        return Ok();
    }
}