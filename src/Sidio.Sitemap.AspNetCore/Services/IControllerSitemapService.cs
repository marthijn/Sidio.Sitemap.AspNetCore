using Sidio.Sitemap.Core;

namespace Sidio.Sitemap.AspNetCore.Services;

/// <summary>
/// The controller sitemap service interface.
/// </summary>
public interface IControllerSitemapService
{
    /// <summary>
    /// Returns a collection of sitemap nodes for the specified controller type.
    /// </summary>
    /// <param name="controllerType">The controller type.</param>
    /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="SitemapNode"/> objects.</returns>
    IEnumerable<SitemapNode> CreateSitemap(Type controllerType);
}