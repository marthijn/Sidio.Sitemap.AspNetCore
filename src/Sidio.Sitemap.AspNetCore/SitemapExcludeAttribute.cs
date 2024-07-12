using Sidio.Sitemap.AspNetCore.Middleware;

namespace Sidio.Sitemap.AspNetCore;

/// <summary>
/// The sitemap exclude attribute.
/// When the <see cref="EndpointInclusionMode"/> set to <see cref="EndpointInclusionMode.OptIn"/>, this attribute can be used to exclude a controller or action from the sitemap.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class SitemapExcludeAttribute : Attribute;