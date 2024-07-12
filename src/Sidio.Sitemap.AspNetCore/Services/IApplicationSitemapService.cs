namespace Sidio.Sitemap.AspNetCore.Services;

/// <summary>
/// The application sitemap service. Responsible for creating the sitemap XML for the application.
/// </summary>
public interface IApplicationSitemapService
{
    /// <summary>
    /// Create a sitemap XML string.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="string"/> representing the sitemap XML.</returns>
    Task<string> CreateSitemapAsync(CancellationToken cancellationToken = default);
}