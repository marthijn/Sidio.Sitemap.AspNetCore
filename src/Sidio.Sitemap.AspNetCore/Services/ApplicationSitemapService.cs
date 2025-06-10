using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sidio.Sitemap.AspNetCore.Middleware;
using Sidio.Sitemap.Core;
using Sidio.Sitemap.Core.Services;

namespace Sidio.Sitemap.AspNetCore.Services;

/// <summary>
/// The application sitemap service.
/// </summary>
public sealed class ApplicationSitemapService : IApplicationSitemapService
{
    private const string CacheKey = $"{nameof(ApplicationSitemapService)}:Sitemap";

    private readonly ISitemapService _sitemapService;
    private readonly IControllerSitemapService _controllerSitemapService;
    private readonly HybridCache? _cache;
    private readonly IOptions<SitemapMiddlewareOptions> _options;
    private readonly IControllerService _controllerService;
    private readonly IRazorPageSitemapService _razorPageSitemapService;
    private readonly ILogger<ApplicationSitemapService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationSitemapService"/> class.
    /// </summary>
    /// <param name="sitemapService">The sitemap service.</param>
    /// <param name="controllerSitemapService">The controller sitemap service.</param>
    /// <param name="cache">The cache.</param>
    /// <param name="options">Options.</param>
    /// <param name="controllerService">The controller service.</param>
    /// <param name="razorPageSitemapService">The razor pages sitemap service.</param>
    /// <param name="logger">The logger.</param>
    public ApplicationSitemapService(
        ISitemapService sitemapService,
        IControllerSitemapService controllerSitemapService,
        HybridCache cache,
        IOptions<SitemapMiddlewareOptions> options,
        IControllerService controllerService,
        IRazorPageSitemapService razorPageSitemapService,
        ILogger<ApplicationSitemapService> logger) : this(
        sitemapService,
        controllerSitemapService,
        options,
        controllerService,
        razorPageSitemapService,
        logger)
    {
        _cache = cache;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationSitemapService"/> class.
    /// </summary>
    /// <param name="sitemapService">The sitemap service.</param>
    /// <param name="controllerSitemapService">The controller sitemap service.</param>
    /// <param name="options">Options.</param>
    /// <param name="controllerService">The controller service.</param>
    /// <param name="razorPageSitemapService">The razor pages sitemap service.</param>
    /// <param name="logger">The logger.</param>
    public ApplicationSitemapService(
        ISitemapService sitemapService,
        IControllerSitemapService controllerSitemapService,
        IOptions<SitemapMiddlewareOptions> options,
        IControllerService controllerService,
        IRazorPageSitemapService razorPageSitemapService,
        ILogger<ApplicationSitemapService> logger)
    {
        _sitemapService = sitemapService;
        _controllerSitemapService = controllerSitemapService;
        _options = options;
        _controllerService = controllerService;
        _razorPageSitemapService = razorPageSitemapService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<string> CreateSitemapAsync(CancellationToken cancellationToken = default)
    {
        if (!CacheEnabled)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace("Cache is disabled, creating sitemap without caching");
            }

            return await CreateSitemapInternalAsync(cancellationToken).ConfigureAwait(false);
        }

        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace("Cache is enabled, trying to get sitemap from cache by key `{CacheKey}`", CacheKey);
        }

        var xml = await _cache.GetOrCreateAsync(
            CacheKey,
            async ct =>
            {
                var xmlSiteMap = await CreateSitemapInternalAsync(ct).ConfigureAwait(false);
                if (_logger.IsEnabled(LogLevel.Trace))
                {
                    _logger.LogTrace("Sitemap created and cached, returning sitemap");
                }

                return xmlSiteMap;
            },
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(_options.Value.CacheDurationInMinutes),
                LocalCacheExpiration = TimeSpan.FromMinutes(_options.Value.LocalCacheDurationInMinutes),
            },
            cancellationToken: cancellationToken);
        return xml;
    }

    private Task<string> CreateSitemapInternalAsync(CancellationToken cancellationToken = default)
    {
        var sitemap = CreateSitemapObject();
        return _sitemapService.SerializeAsync(sitemap, cancellationToken);
    }

    private Core.Sitemap CreateSitemapObject()
    {
        var controllers = _controllerService.GetControllersFromAssembly(_options.Value.AssemblyMarker);

        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace("Found {ControllerCount} controllers", controllers.Count);
        }

        var nodes = new HashSet<SitemapNode>();
        foreach (var controllerNodes in controllers.Select(controller => _controllerSitemapService.CreateSitemap(controller)))
        {
            nodes.UnionWith(controllerNodes);
        }

        var razorPages = _razorPageSitemapService.CreateSitemap();
        if (razorPages.Count > 0)
        {
            nodes.UnionWith(razorPages);
        }

        if (_options.Value.IncludeApiControllers)
        {
            var apiControllers = _controllerService.GetControllerBasesFromAssembly(_options.Value.AssemblyMarker);
            foreach (var apiController in apiControllers.Select(
                         controller => _controllerSitemapService.CreateSitemap(controller)))
            {
                nodes.UnionWith(apiController);
            }
        }

        return new (nodes);
    }

    [MemberNotNullWhen(true, nameof(_cache))]
    private bool CacheEnabled => _options.Value.CacheEnabled && _cache != null;
}