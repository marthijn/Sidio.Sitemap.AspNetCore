using Sidio.Sitemap.Core;

namespace Sidio.Sitemap.AspNetCore.Services;

/// <summary>
/// The controller sitemap service interface. Responsible for creating sitemap nodes for controllers.
/// </summary>
public interface IControllerSitemapService
{
    /// <summary>
    /// Returns a collection of sitemap nodes for the specified controller type.
    /// </summary>
    /// <param name="controllerType">The controller type.</param>
    /// <returns>A <see cref="IReadOnlySet{T}"/> of <see cref="SitemapNode"/> objects.</returns>
    IReadOnlySet<SitemapNode> CreateSitemap(Type controllerType);
}