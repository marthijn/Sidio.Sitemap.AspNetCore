namespace Sidio.Sitemap.AspNetCore.Middleware;

/// <summary>
/// The sitemap middleware options.
/// </summary>
public sealed class SitemapMiddlewareOptions
{
    /// <summary>
    /// Gets or sets the endpoint inclusion method.
    /// </summary>
    public EndpointInclusionMethod EndpointInclusionMethod { get; set; } = EndpointInclusionMethod.OptIn;

    /// <summary>
    /// Gets or sets a value indicating whether the cache is enabled.
    /// </summary>
    public bool CacheEnabled { get; set; }

    /// <summary>
    /// Gets or sets the cache absolute expiration in minutes.
    /// </summary>
    public int CacheAbsoluteExpirationInMinutes { get; set; } = 60;
}