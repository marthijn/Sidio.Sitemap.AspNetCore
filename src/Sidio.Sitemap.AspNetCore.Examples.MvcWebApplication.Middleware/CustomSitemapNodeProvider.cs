using Sidio.Sitemap.AspNetCore.Middleware;
using Sidio.Sitemap.Core;

namespace Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Middleware;

public sealed class CustomSitemapNodeProvider : ICustomSitemapNodeProvider
{
    public IEnumerable<SitemapNode> GetNodes()
    {
        yield return new SitemapNode("/custom-sitemap-node-1");
    }
}