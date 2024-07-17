using Sidio.Sitemap.AspNetCore.Middleware;

namespace Sidio.Sitemap.AspNetCore;

/// <summary>
/// The sitemap exclude attribute.
/// When the <see cref="EndpointInclusionMode"/> set to <see cref="EndpointInclusionMode.OptOut"/>, this attribute can be used to exclude a controller, action or page from the sitemap.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class SitemapExcludeAttribute : Attribute;