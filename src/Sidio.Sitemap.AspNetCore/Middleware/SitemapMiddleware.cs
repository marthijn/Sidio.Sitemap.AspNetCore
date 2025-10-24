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

    public Task InvokeAsync(HttpContext context)
    {
        if (HttpMethods.IsGet(context.Request.Method) && context.Request.Path == SitemapPath)
        {
            return HandleSitemapRequestAsync(context);
        }

        return _next(context);
    }

    private static async Task HandleSitemapRequestAsync(HttpContext context)
    {
        var resolverService = context.RequestServices.GetRequiredService<IApplicationSitemapService>();
        var xml = await resolverService.CreateSitemapAsync().ConfigureAwait(false);

        context.Response.ContentType = SitemapResult.ContentType;
        await context.Response.WriteAsync(xml, Encoding.UTF8).ConfigureAwait(false);
    }
}