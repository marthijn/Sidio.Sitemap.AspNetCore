using System.Diagnostics;
using Sidio.Sitemap.Core;

namespace Sidio.Sitemap.AspNetCore;

/// <summary>
/// The Sitemap extensions.
/// </summary>
public static class SitemapExtensions
{
    /// <summary>
    /// Adds a nullable URL to the sitemap.
    /// </summary>
    /// <param name="sitemap">The sitemap.</param>
    /// <param name="url">The URL.</param>
    /// <returns>Returns <c>true</c> when the url was added.</returns>
    public static bool TryAdd(this Core.Sitemap sitemap, string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        return sitemap.Add(new SitemapNode(url)) == 1;
    }

    /// <summary>
    /// Adds a range of nullable URLs to the sitemap.
    /// </summary>
    /// <param name="sitemap">The sitemap.</param>
    /// <param name="urls">The urls.</param>
    /// <returns>Returns actual the number of nodes that were added.</returns>
    public static int TryAdd(this Core.Sitemap sitemap, params string?[] urls)
    {
        var nonNullableUrls = urls.Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => new SitemapNode(x ?? throw new UnreachableException())).Cast<ISitemapNode>().ToArray();

        return nonNullableUrls.Length > 0 ? sitemap.Add(nonNullableUrls) : 0;
    }
}