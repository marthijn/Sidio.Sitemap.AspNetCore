using Sidio.Sitemap.Core;

namespace Sidio.Sitemap.AspNetCore.Middleware;

/// <summary>
/// A custom sitemap node provider to provide additional sitemap nodes.
/// </summary>
public interface ICustomSitemapNodeProvider
{
    /// <summary>
    /// Retrieves the sitemap nodes.
    /// </summary>
    /// <returns>The sitemap nodes.</returns>
    IEnumerable<SitemapNode> GetNodes();
}