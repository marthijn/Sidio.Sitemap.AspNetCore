using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Sidio.Sitemap.Core;

namespace Sidio.Sitemap.AspNetCore.AutoSitemap;

public sealed class EndpointResolverService
{
    private readonly IUrlHelperFactory _urlHelperFactory;

    private readonly IActionContextAccessor _actionContextAccessor;

    private readonly ILogger<EndpointResolverService> _logger;

    private readonly List<SitemapNode> _nodes = new ();

    public EndpointResolverService(
        IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionContextAccessor,
        ILogger<EndpointResolverService> logger)
    {
        _urlHelperFactory = urlHelperFactory;
        _actionContextAccessor = actionContextAccessor;
        _logger = logger;
    }

    public void AddController<TController>(EndpointInclusionMethod inclusionMethod = EndpointInclusionMethod.OptOut)
        where TController : Controller =>
        AddController(typeof(TController), inclusionMethod);

    public void AddAllControllers(EndpointInclusionMethod inclusionMethod = EndpointInclusionMethod.OptOut)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var types = currentAssembly.GetTypes();
        foreach(var t in types.Where(x => x.BaseType == typeof(Controller)))
        {
            AddController(t, inclusionMethod);
        }
    }

    public Core.Sitemap ToSitemap() => new (_nodes);

    public SitemapResult ToSitemapResult() => new (ToSitemap());

    private IUrlHelper GetUrlHelper() =>
        _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext ?? throw new InvalidOperationException("ActionContext is null"));

    private void AddController(Type controllerType, EndpointInclusionMethod inclusionMethod = EndpointInclusionMethod.OptOut)
    {
        switch (inclusionMethod)
        {
            case EndpointInclusionMethod.OptIn:
                AddControllerOptIn(controllerType);
                break;
            case EndpointInclusionMethod.OptOut:
                AddControllerOptOut(controllerType);
                break;
            default:
                throw new NotImplementedException($"EndpointInclusionMethod {inclusionMethod} is not implemented");
        }
    }

    private void AddControllerOptIn(Type controllerType)
    {
        var hasOptInAttribute = controllerType.GetCustomAttributes<SitemapIncludeAttribute>().Any();

        var urlHelper = GetUrlHelper();
        var methods = controllerType.GetMethods();
        foreach (var method in methods)
        {
            var includeFunction = hasOptInAttribute || method.GetCustomAttributes<SitemapIncludeAttribute>().Any();
            if (includeFunction)
            {
                _nodes.Add(new SitemapNode(urlHelper.Action(method.Name, controllerType.Name)));
                _logger.LogTrace("Added method {MethodName} in controller {ControllerType} to sitemap", method.Name, controllerType.Name);
            }
            else
            {
                _logger.LogTrace(
                    "Method {MethodName} in controller {ControllerType} has not SitemapIncludeAttribute, skipping",
                    method.Name,
                    controllerType.Name);
            }
        }
    }

    private void AddControllerOptOut(Type controllerType, EndpointInclusionMethod inclusionMethod = EndpointInclusionMethod.OptOut)
    {
        var hasOptOutAttribute = controllerType.GetCustomAttributes<SitemapExcludeAttribute>().Any();
        if (hasOptOutAttribute)
        {
            _logger.LogTrace("Controller {ControllerType} has SitemapExcludeAttribute, skipping", controllerType.Name);
            return;
        }

        var urlHelper = GetUrlHelper();
        var methods = controllerType.GetMethods();
        foreach (var method in methods)
        {
            var excludeFunction = method.GetCustomAttributes<SitemapExcludeAttribute>().Any();
            if (!excludeFunction)
            {
                _nodes.Add(new SitemapNode(urlHelper.Action(method.Name, controllerType.Name)));
                _logger.LogTrace("Added method {MethodName} in controller {ControllerType} to sitemap", method.Name, controllerType.Name);
            }
            else
            {
                _logger.LogTrace(
                    "Method {MethodName} in controller {ControllerType} has SitemapExcludeAttribute, skipping",
                    method.Name,
                    controllerType.Name);
            }
        }
    }
}