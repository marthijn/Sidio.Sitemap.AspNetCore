using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sidio.Sitemap.AspNetCore.Middleware;
using Sidio.Sitemap.Core;

namespace Sidio.Sitemap.AspNetCore.Services;

/// <summary>
/// The controller sitemap service.
/// </summary>
public sealed class ControllerSitemapService : IControllerSitemapService
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptions<SitemapMiddlewareOptions> _options;
    private readonly ILogger<ControllerSitemapService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ControllerSitemapService"/> class.
    /// </summary>
    /// <param name="linkGenerator">The link generator.</param>
    /// <param name="actionDescriptorCollectionProvider">The action description collection provider.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="options">The options.</param>
    /// <param name="logger">The logger.</param>
    public ControllerSitemapService(
        LinkGenerator linkGenerator,
        IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
        IHttpContextAccessor httpContextAccessor,
        IOptions<SitemapMiddlewareOptions> options,
        ILogger<ControllerSitemapService> logger
    )
    {
        _linkGenerator = linkGenerator;
        _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        _httpContextAccessor = httpContextAccessor;
        _options = options;
        _logger = logger;
    }

    /// <inheritdoc />
    public IReadOnlySet<SitemapNode> CreateSitemap(Type controllerType)
    {
        var inclusionMethod = _options.Value.EndpointInclusionMode;
        var actions = _actionDescriptorCollectionProvider.ActionDescriptors.Items
            .OfType<ControllerActionDescriptor>()
            .Where(x => IsSitemapController(x.ControllerTypeInfo))
            .ToList();

        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace(
                "Found {Count} actions in controller `{ControllerType}`",
                actions.Count,
                controllerType.Name);
        }

        var nodes = new HashSet<SitemapNode>();
        if (actions.Count == 0)
        {
            return nodes;
        }

        if (inclusionMethod == EndpointInclusionMode.OptIn)
        {
            var methods = GetControllerMethodsOptIn(
                controllerType,
                actions.Where(x => IsSitemapController(x.ControllerTypeInfo)));
            nodes.UnionWith(methods);
        }
        else
        {
            var methods = GetControllerMethodsOptOut(
                controllerType,
                actions.Where(x => IsSitemapController(x.ControllerTypeInfo)));
            nodes.UnionWith(methods);
        }

        return nodes;
    }

    private static bool IsSitemapController(TypeInfo typeInfo) =>
        typeInfo.BaseType == typeof(Controller) || typeInfo.BaseType == typeof(ControllerBase);

    private HttpContext HttpContext => _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is null");

    private SitemapNode? CreateNode(ControllerActionDescriptor action)
    {
        var url = _linkGenerator.GetUriByAction(HttpContext, action.ActionName, action.ControllerName);

        if (_logger.IsEnabled(LogLevel.Warning) && string.IsNullOrWhiteSpace(url))
        {
            _logger.LogWarning(
                "Unable to create sitemap URL for action `{Action}` in controller `{Controller}`",
                action.ActionName,
                action.ControllerName);
        }

        if (_logger.IsEnabled(LogLevel.Trace) && !string.IsNullOrWhiteSpace(url))
        {
            _logger.LogTrace(
                "Created sitemap URL for action `{Action}` in controller `{Controller}`",
                action.ActionName,
                action.ControllerName);

        }

        return !string.IsNullOrWhiteSpace(url) ? new SitemapNode(url) : null;
    }

    private HashSet<SitemapNode> GetControllerMethodsOptIn(Type controllerType, IEnumerable<ControllerActionDescriptor> actions)
    {
        var hasOptInAttribute = controllerType.GetCustomAttributes<SitemapIncludeAttribute>().Any();

        var nodes = new HashSet<SitemapNode>();
        foreach (var action in actions)
        {
            var includeFunction = hasOptInAttribute || action.EndpointMetadata.Any(x => x is SitemapIncludeAttribute);
            if (includeFunction)
            {
                var node = CreateNode(action);
                if (node != null)
                {
                    nodes.Add(node);
                }
            }
            else
            {
                if (_logger.IsEnabled(LogLevel.Trace))
                {
                    _logger.LogTrace(
                        "Action `{Action}` in controller `{Controller}` is not decorated with SitemapIncludeAttribute, skipping",
                        action.ActionName,
                        action.ControllerName);
                }
            }
        }

        return nodes;
    }

    private HashSet<SitemapNode> GetControllerMethodsOptOut(Type controllerType, IEnumerable<ControllerActionDescriptor> actions)
    {
        var nodes = new HashSet<SitemapNode>();
        var hasOptOutAttribute = controllerType.GetCustomAttributes<SitemapExcludeAttribute>().Any();
        if (hasOptOutAttribute)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(
                    "Controller `{ControllerType}` is decorated with SitemapExcludeAttribute, skipping",
                    controllerType.Name);
            }

            return nodes;
        }

        foreach (var action in actions)
        {
            var excludeFunction = action.EndpointMetadata.Any(x => x is SitemapExcludeAttribute);
            if (!excludeFunction)
            {
                var node = CreateNode(action);
                if (node != null)
                {
                    nodes.Add(node);
                }
            }
            else
            {
                if (_logger.IsEnabled(LogLevel.Trace))
                {
                    _logger.LogTrace(
                        "Action `{Action}` in controller `{Controller}` is decorated with SitemapExcludeAttribute, skipping",
                        action.ActionName,
                        action.ControllerName);
                }
            }
        }

        return nodes;
    }
}