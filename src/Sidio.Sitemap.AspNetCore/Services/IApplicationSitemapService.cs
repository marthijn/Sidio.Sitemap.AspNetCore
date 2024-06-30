namespace Sidio.Sitemap.AspNetCore.Services;

/// <summary>
/// The application sitemap service.
/// </summary>
public interface IApplicationSitemapService
{
    /// <summary>
    /// Create a sitemap XML string.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="string"/>.</returns>
    Task<string> CreateSitemapAsync(CancellationToken cancellationToken = default);
}