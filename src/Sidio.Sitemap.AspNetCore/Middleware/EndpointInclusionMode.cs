namespace Sidio.Sitemap.AspNetCore.Middleware;

/// <summary>
/// The endpoint inclusion mode.
/// </summary>
public enum EndpointInclusionMode
{
    /// <summary>
    /// By using OptIn, only the controllers or actions with the <see cref="SitemapIncludeAttribute"/> will be included in the sitemap.
    /// </summary>
    OptIn,

    /// <summary>
    /// By using OptOut, all controllers or actions will be included in the sitemap except the ones with the <see cref="SitemapExcludeAttribute"/>.
    /// </summary>
    OptOut,
}