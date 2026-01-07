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
        serviceCollection.AddScoped<IRazorPageSitemapService, RazorPagesSitemapService>();
        serviceCollection.AddScoped<IApplicationSitemapService, ApplicationSitemapService>();
        return serviceCollection;
    }

    /// <summary>
    /// Adds a custom sitemap node provider which will be used to provide additional sitemap nodes.
    /// This will only work when the middleware is added via <see cref="AddSitemapMiddleware(IServiceCollection, Action{SitemapMiddlewareOptions})"/>.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="serviceLifetime">The service lifetime.</param>
    /// <typeparam name="T">The implementation of <see cref="IServiceCollection"/>.</typeparam>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddCustomSitemapNodeProvider<T>(this IServiceCollection serviceCollection, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where T : class, ICustomSitemapNodeProvider
    {
        var serviceDescriptor = new ServiceDescriptor(typeof(ICustomSitemapNodeProvider), typeof(T), serviceLifetime);
        serviceCollection.Add(serviceDescriptor);
        return serviceCollection;
    }
}