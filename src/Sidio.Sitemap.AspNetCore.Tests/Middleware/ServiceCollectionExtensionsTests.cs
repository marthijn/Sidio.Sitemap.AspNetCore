using Microsoft.Extensions.DependencyInjection;
using Sidio.Sitemap.AspNetCore.Middleware;
using Sidio.Sitemap.AspNetCore.Services;
using Sidio.Sitemap.Core;

namespace Sidio.Sitemap.AspNetCore.Tests.Middleware;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddSitemapMiddleware_WhenCalled_ShouldRegisterRequiredServices()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddSitemapMiddleware(options => { });

        // assert
        services.Should().Contain(x => x.ServiceType == typeof(IControllerService) && x.Lifetime == ServiceLifetime.Scoped);
        services.Should().Contain(x => x.ServiceType == typeof(IControllerSitemapService) && x.Lifetime == ServiceLifetime.Scoped);
        services.Should().Contain(x => x.ServiceType == typeof(IRazorPageSitemapService) && x.Lifetime == ServiceLifetime.Scoped);
        services.Should().Contain(x => x.ServiceType == typeof(IApplicationSitemapService) && x.Lifetime == ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddSitemapMiddleware_WhenCalled_ShouldConfigureOptions()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddSitemapMiddleware(options =>
        {
            options.CacheEnabled = true;
            options.CacheDurationInMinutes = 30;
        });

        // assert
        var serviceProvider = services.BuildServiceProvider();
        var optionsAccessor = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<SitemapMiddlewareOptions>>();
        optionsAccessor.Value.CacheEnabled.Should().BeTrue();
        optionsAccessor.Value.CacheDurationInMinutes.Should().Be(30);
    }

    [Fact]
    public void AddSitemapMiddleware_WhenCalled_ShouldReturnServiceCollection()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        var result = services.AddSitemapMiddleware(options => { });

        // assert
        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddCustomSitemapNodeProvider_WithDefaultLifetime_ShouldRegisterWithScopedLifetime()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddCustomSitemapNodeProvider<TestCustomSitemapNodeProvider>();

        // assert
        services.Should().Contain(x =>
            x.ServiceType == typeof(ICustomSitemapNodeProvider) &&
            x.ImplementationType == typeof(TestCustomSitemapNodeProvider) &&
            x.Lifetime == ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddCustomSitemapNodeProvider_WithTransientLifetime_ShouldRegisterWithTransientLifetime()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddCustomSitemapNodeProvider<TestCustomSitemapNodeProvider>(ServiceLifetime.Transient);

        // assert
        services.Should().Contain(x =>
            x.ServiceType == typeof(ICustomSitemapNodeProvider) &&
            x.ImplementationType == typeof(TestCustomSitemapNodeProvider) &&
            x.Lifetime == ServiceLifetime.Transient);
    }

    [Fact]
    public void AddCustomSitemapNodeProvider_WithSingletonLifetime_ShouldRegisterWithSingletonLifetime()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddCustomSitemapNodeProvider<TestCustomSitemapNodeProvider>(ServiceLifetime.Singleton);

        // assert
        services.Should().Contain(x =>
            x.ServiceType == typeof(ICustomSitemapNodeProvider) &&
            x.ImplementationType == typeof(TestCustomSitemapNodeProvider) &&
            x.Lifetime == ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddCustomSitemapNodeProvider_WhenCalled_ShouldReturnServiceCollection()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        var result = services.AddCustomSitemapNodeProvider<TestCustomSitemapNodeProvider>();

        // assert
        result.Should().BeSameAs(services);
    }

    private sealed class TestCustomSitemapNodeProvider : ICustomSitemapNodeProvider
    {
        public IEnumerable<SitemapNode> GetNodes()
        {
            return new List<SitemapNode> { new("/test") };
        }
    }
}