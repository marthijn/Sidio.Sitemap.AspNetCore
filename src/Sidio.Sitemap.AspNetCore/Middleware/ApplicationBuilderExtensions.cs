using Microsoft.AspNetCore.Builder;

namespace Sidio.Sitemap.AspNetCore.Middleware;

/// <summary>
/// The application builder extensions.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Uses the sitemap middleware.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
    public static IApplicationBuilder UseSitemap(this IApplicationBuilder app) => app.UseMiddleware<SitemapMiddleware>();
}