using Microsoft.Extensions.DependencyInjection;
using Sidio.Sitemap.AspNetCore.Services;

namespace Sidio.Sitemap.AspNetCore.Middleware;

/// <summary>
/// The service collection extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the sitemap middleware.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="options">The options.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddSitemapMiddleware(this IServiceCollection serviceCollection, Action<SitemapMiddlewareOptions> options)
    {
        serviceCollection.Configure(options);
        serviceCollection.AddScoped<IControllerService, ControllerService>();
        serviceCollection.AddScoped<IControllerSitemapService, ControllerSitemapService>();
        serviceCollection.AddScoped<IApplicationSitemapService, ApplicationSitemapService>();
        return serviceCollection;
    }
}