using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sidio.Sitemap.AspNetCore.Middleware;
using Sidio.Sitemap.Core;

namespace Sidio.Sitemap.AspNetCore.Services;

/// <summary>
/// The razor page sitemap service.
/// </summary>
public sealed class RazorPagesSitemapService : IRazorPageSitemapService
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptions<SitemapMiddlewareOptions> _options;
    private readonly ILogger<RazorPagesSitemapService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RazorPagesSitemapService"/> class.
    /// </summary>
    /// <param name="linkGenerator">The link generator.</param>
    /// <param name="actionDescriptorCollectionProvider">The action descriptor collection provider.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="options">The options.</param>
    /// <param name="logger">The logger.</param>
    public RazorPagesSitemapService(LinkGenerator linkGenerator,
        IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
        IHttpContextAccessor httpContextAccessor,
        IOptions<SitemapMiddlewareOptions> options,
        ILogger<RazorPagesSitemapService> logger)
    {
        _linkGenerator = linkGenerator;
        _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        _httpContextAccessor = httpContextAccessor;
        _options = options;
        _logger = logger;
    }

    private SitemapNode CreateNode(CompiledPageActionDescriptor action)
    {
        var url = _linkGenerator.GetUriByPage(HttpContext, action.ViewEnginePath);

        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace(
                "Created sitemap URL for page `{ViewEnginePath}`",
                action.ViewEnginePath);
        }

        return new SitemapNode(url);
    }

    /// <inheritdoc />
    public IReadOnlySet<SitemapNode> CreateSitemap()
    {
        var inclusionMethod = _options.Value.EndpointInclusionMode;
        var actions = _actionDescriptorCollectionProvider.ActionDescriptors.Items
            .OfType<CompiledPageActionDescriptor>()
            .ToList();

        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace(
                "Found {Count} razor pages",
                actions.Count);
        }

        var nodes = new HashSet<SitemapNode>();
        if (actions.Count == 0)
        {
            return nodes;
        }

        if (inclusionMethod == EndpointInclusionMode.OptIn)
        {
            var methods = GetRazorPagesOptIn(actions);
            nodes.UnionWith(methods);
        }
        else
        {
            var methods = GetRazorPagesOptOut(actions);
            nodes.UnionWith(methods);
        }

        return nodes;
    }

    private HttpContext HttpContext => _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is null");

    private HashSet<SitemapNode> GetRazorPagesOptIn(IEnumerable<CompiledPageActionDescriptor> actions)
    {
        var nodes = new HashSet<SitemapNode>();
        foreach (var action in actions)
        {
            var includeFunction = action.EndpointMetadata.Any(x => x is SitemapIncludeAttribute);
            if (includeFunction)
            {
                var node = CreateNode(action);
                nodes.Add(node);
            }
            else
            {
                if (_logger.IsEnabled(LogLevel.Trace))
                {
                    _logger.LogTrace(
                        "Page `{ViewEnginePath}` is not decorated with SitemapIncludeAttribute, skipping",
                        action.ViewEnginePath);
                }
            }
        }

        return nodes;
    }

    private HashSet<SitemapNode> GetRazorPagesOptOut(IEnumerable<CompiledPageActionDescriptor> actions)
    {
        var nodes = new HashSet<SitemapNode>();
        foreach (var action in actions)
        {
            var excludeFunction = action.EndpointMetadata.Any(x => x is SitemapExcludeAttribute);
            if (!excludeFunction)
            {
                var node = CreateNode(action);
                nodes.Add(node);
            }
            else
            {
                if (_logger.IsEnabled(LogLevel.Trace))
                {
                    _logger.LogTrace(
                        "Page `{ViewEnginePath}` is not decorated with SitemapExcludeAttribute, skipping",
                        action.ViewEnginePath);
                }
            }
        }

        return nodes;
    }
}