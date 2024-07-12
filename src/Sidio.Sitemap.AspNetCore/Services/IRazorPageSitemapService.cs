using Sidio.Sitemap.Core;

namespace Sidio.Sitemap.AspNetCore.Services;

/// <summary>
/// The razor page sitemap service.
/// </summary>
public interface IRazorPageSitemapService
{
    /// <summary>
    /// Returns a collection of sitemap nodes for razor pages.
    /// </summary>
    /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="SitemapNode"/> objects.</returns>
    IEnumerable<SitemapNode> CreateSitemap();
}