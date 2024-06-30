﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sidio.Sitemap.AspNetCore.Middleware;
using Sidio.Sitemap.Core;
using Sidio.Sitemap.Core.Services;

namespace Sidio.Sitemap.AspNetCore.Services;

public sealed class ApplicationSitemapService : IApplicationSitemapService
{
    private const string CacheKey = $"{nameof(ApplicationSitemapService)}:Sitemap";

    private readonly ISitemapService _sitemapService;
    private readonly IControllerSitemapService _controllerSitemapService;
    private readonly IDistributedCache? _cache;
    private readonly IOptions<SitemapMiddlewareOptions> _options;
    private readonly IControllerService _controllerService;
    private readonly ILogger<ApplicationSitemapService> _logger;

    public ApplicationSitemapService(
        ISitemapService sitemapService,
        IControllerSitemapService controllerSitemapService,
        IDistributedCache? cache,
        IOptions<SitemapMiddlewareOptions> options,
        IControllerService controllerService,
        ILogger<ApplicationSitemapService> logger)
    {
        _sitemapService = sitemapService;
        _controllerSitemapService = controllerSitemapService;
        _cache = cache;
        _options = options;
        _controllerService = controllerService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<string> CreateSitemapAsync(CancellationToken cancellationToken = default)
    {
        if (!CacheEnabled)
        {
            _logger.LogTrace("Cache is disabled, creating sitemap without caching");
            return await CreateSitemapInternalAsync(cancellationToken).ConfigureAwait(false);
        }

        _logger.LogTrace("Cache is enabled, trying to get sitemap from cache");

        var xml = await _cache.GetStringAsync(CacheKey, cancellationToken).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(xml))
        {
            _logger.LogTrace("Sitemap found in cache, returning cached sitemap");
            return xml;
        }

        xml = await CreateSitemapInternalAsync(cancellationToken).ConfigureAwait(false);
        await _cache.SetStringAsync(CacheKey, xml, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.Value.CacheAbsoluteExpirationInMinutes)
        }, cancellationToken);

        _logger.LogTrace("Sitemap created and cached, returning sitemap");
        return xml;
    }

    private Task<string> CreateSitemapInternalAsync(CancellationToken cancellationToken = default)
    {
        var sitemap = CreateSitemapObject();
        return _sitemapService.SerializeAsync(sitemap, cancellationToken);
    }

    private Core.Sitemap CreateSitemapObject()
    {
        var controllers = _controllerService.GetControllersFromEntryAssembly();

        _logger.LogTrace("Found {ControllerCount} controllers", controllers.Count);

        var nodes = new HashSet<SitemapNode>();
        foreach (var controllerNodes in controllers.Select(controller => _controllerSitemapService.CreateSitemap(controller)))
        {
            nodes.UnionWith(controllerNodes);
        }

        return new (nodes);
    }

    [MemberNotNullWhen(true, nameof(_cache))]
    private bool CacheEnabled => _options.Value.CacheEnabled && _cache != null;
}