using Microsoft.AspNetCore.Mvc;

namespace Sidio.Sitemap.AspNetCore.Middleware;

/// <summary>
/// The sitemap middleware options.
/// </summary>
public sealed class SitemapMiddlewareOptions
{
    /// <summary>
    /// Gets or sets the endpoint inclusion method.
    /// </summary>
    public EndpointInclusionMode EndpointInclusionMode { get; set; } = EndpointInclusionMode.OptIn;

    /// <summary>
    /// Gets or sets a value indicating whether the cache is enabled.
    /// </summary>
    public bool CacheEnabled { get; set; }

    /// <summary>
    /// Gets or sets the cache duration in minutes.
    /// </summary>
    public int CacheDurationInMinutes { get; set; } = 60;

    /// <summary>
    /// Gets or sets the local cache duration in minutes.
    /// </summary>
    public int LocalCacheDurationInMinutes { get; set; } = 5;

    /// <summary>
    /// Gets or sets the assembly marker type from which to retrieve controllers.
    /// When null, the entry assembly is used.
    /// </summary>
    public Type? AssemblyMarker { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include API controllers (types derived from <see cref="ControllerBase"/>).
    /// </summary>
    public bool IncludeApiControllers { get; set; }
}