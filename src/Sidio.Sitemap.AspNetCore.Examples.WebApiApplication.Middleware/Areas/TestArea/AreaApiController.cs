using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Sidio.Sitemap.AspNetCore.Examples.WebApiApplication.Middleware.Areas.TestArea;

[ApiController]
[Area("TestArea")]
[Route("[area]/[controller]/[action]")]
[ExcludeFromCodeCoverage]
public sealed class AreaApiController : ControllerBase
{
    [HttpGet]
    [SitemapInclude]
    public IActionResult Included()
    {
        return Ok();
    }

    [HttpGet]
    [SitemapExclude]
    public IActionResult Excluded()
    {
        return Ok();
    }
}
