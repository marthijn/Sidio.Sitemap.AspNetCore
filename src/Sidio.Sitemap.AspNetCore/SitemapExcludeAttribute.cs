namespace Sidio.Sitemap.AspNetCore;

/// <summary>
/// The sitemap exclude attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class SitemapExcludeAttribute : Attribute;