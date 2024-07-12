using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sidio.Sitemap.AspNetCore.Services;

namespace Sidio.Sitemap.AspNetCore.Middleware;

internal sealed class SitemapMiddleware
{
    private static readonly PathString SitemapPath = new("/sitemap.xml");

    private readonly RequestDelegate _next;

    public SitemapMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == SitemapPath)
        {
            var resolverService = context.RequestServices.GetRequiredService<IApplicationSitemapService>();
            var xml = await resolverService.CreateSitemapAsync().ConfigureAwait(false);

            context.Response.ContentType = SitemapResult.ContentType;
            await context.Response.WriteAsync(xml, Encoding.UTF8).ConfigureAwait(false);

            return;
        }

        await _next(context).ConfigureAwait(false);
    }
}