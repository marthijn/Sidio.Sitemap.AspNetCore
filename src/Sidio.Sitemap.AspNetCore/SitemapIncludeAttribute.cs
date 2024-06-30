namespace Sidio.Sitemap.AspNetCore;

/// <summary>
/// The sitemap include attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class SitemapIncludeAttribute : Attribute;