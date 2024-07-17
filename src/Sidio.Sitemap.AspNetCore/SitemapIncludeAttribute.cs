using Sidio.Sitemap.AspNetCore.Middleware;

namespace Sidio.Sitemap.AspNetCore;

/// <summary>
/// The sitemap include attribute.
/// When the <see cref="EndpointInclusionMode"/> set to <see cref="EndpointInclusionMode.OptIn"/>, this attribute can be used to include a controller, action or page in the sitemap.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class SitemapIncludeAttribute : Attribute;