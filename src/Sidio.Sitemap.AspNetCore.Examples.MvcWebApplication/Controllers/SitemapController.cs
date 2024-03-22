using Microsoft.AspNetCore.Mvc;
using Sidio.Sitemap.Core;

namespace Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Controllers;

public sealed class SitemapController : Controller
{
    [Route("sitemap.xml")]
    public IActionResult SitemapIndex()
    {
        var sitemap = new SitemapIndex();
        sitemap.Add(new SitemapIndexNode(Url.Action("SitemapHome")));

        return new SitemapResult(sitemap);
    }

    [Route("sitemap-home.xml")]
    public IActionResult SitemapHome()
    {
        var sitemap = new Core.Sitemap();
        sitemap.Add(new SitemapNode(Url.Action("Index", "Home")));
        sitemap.Add(new SitemapNode(Url.Action("Privacy", "Home")));

        return new SitemapResult(sitemap);
    }
}