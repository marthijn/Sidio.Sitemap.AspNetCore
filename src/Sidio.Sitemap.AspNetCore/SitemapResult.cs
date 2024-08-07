﻿using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sidio.Sitemap.Core;
using Sidio.Sitemap.Core.Services;

namespace Sidio.Sitemap.AspNetCore;

/// <summary>
/// The sitemap result.
/// </summary>
public sealed class SitemapResult : ActionResult
{
    internal const string ContentType = "application/xml";

    private readonly SitemapIndex? _sitemapIndex;

    private readonly Core.Sitemap? _sitemap;

    /// <summary>
    /// Initializes a new instance of the <see cref="SitemapResult"/> class.
    /// </summary>
    /// <param name="sitemap">The sitemap.</param>
    public SitemapResult(Core.Sitemap sitemap)
    {
        ArgumentNullException.ThrowIfNull(sitemap);
        _sitemap = sitemap;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SitemapResult"/> class.
    /// </summary>
    /// <param name="sitemapIndex">The sitemap index.</param>
    public SitemapResult(SitemapIndex sitemapIndex)
    {
        ArgumentNullException.ThrowIfNull(sitemapIndex);
        _sitemapIndex = sitemapIndex;
    }

    /// <inheritdoc />
    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var xml = await SerializeAsync(context.HttpContext).ConfigureAwait(false);

        var response = context.HttpContext.Response;
        response.ContentType = ContentType;
        await response.WriteAsync(xml, Encoding.UTF8).ConfigureAwait(false);
        await base.ExecuteResultAsync(context).ConfigureAwait(false);
    }

    private Task<string> SerializeAsync(HttpContext httpContext)
    {
        if (_sitemap != null)
        {
            var service = httpContext.RequestServices.GetRequiredService<ISitemapService>();
            return service.SerializeAsync(_sitemap);
        }

        if (_sitemapIndex != null)
        {
            var service = httpContext.RequestServices.GetRequiredService<ISitemapIndexService>();
            return service.SerializeAsync(_sitemapIndex);
        }

        throw new InvalidOperationException("No sitemap or sitemap index provided.");
    }
}