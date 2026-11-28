using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Middleware.Areas.TestArea.Controllers;

[Area("TestArea")]
[ExcludeFromCodeCoverage]
public sealed class AreaHomeController : Controller
{
    [SitemapInclude]
    public IActionResult Included()
    {
        return Content(nameof(Included));
    }

    [SitemapExclude]
    public IActionResult Excluded()
    {
        return Content(nameof(Excluded));
    }
}
