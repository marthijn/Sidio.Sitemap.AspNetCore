using Sidio.Sitemap.Core;

namespace Sidio.Sitemap.AspNetCore;

/// <summary>
/// The Sitemap-index extensions.
/// </summary>
public static class SitemapIndexExtensions
{
    /// <summary>
    /// Adds a nullable URL to the sitemap index.
    /// </summary>
    /// <param name="sitemapIndex">The sitemap index.</param>
    /// <param name="url">The URL.</param>
    /// <returns>Returns <c>true</c> when the url was added.</returns>
    public static bool TryAdd(this SitemapIndex sitemapIndex, string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        return sitemapIndex.Add(new SitemapIndexNode(url)) == 1;
    }
}