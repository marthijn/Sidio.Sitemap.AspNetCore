using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sidio.Sitemap.AspNetCore.Middleware;
using Sidio.Sitemap.AspNetCore.Services;
using Sidio.Sitemap.Core;
using Sidio.Sitemap.Core.Services;

namespace Sidio.Sitemap.AspNetCore.Tests.Services;

public sealed class ApplicationSitemapServiceTests
{
    [Fact]
    public async Task CreateSitemapAsync_WhenCacheDisabled_ShouldReturnSitemap()
    {
        // arrange
        var options = new SitemapMiddlewareOptions
        {
            CacheEnabled = false
        };
        var service = CreateService(options, out var hybridCacheMock, null);

        // act
        var result = await service.CreateSitemapAsync();

        // assert
        result.Should().NotBeNullOrWhiteSpace();
        hybridCacheMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateSitemapAsync_WhenCacheDisabledWithCustomSitemapNodeProvider_ShouldReturnSitemap()
    {
        // arrange
        var options = new SitemapMiddlewareOptions
        {
            CacheEnabled = false
        };

        var customSitemapNodeProviderMock = new Mock<ICustomSitemapNodeProvider>();
        customSitemapNodeProviderMock.Setup(x => x.GetNodes())
            .Returns(new List<SitemapNode> { new ("/custom") });

        var service = CreateService(options, out var hybridCacheMock, customSitemapNodeProviderMock.Object);

        // act
        var result = await service.CreateSitemapAsync();

        // assert
        result.Should().NotBeNullOrWhiteSpace();
        hybridCacheMock.VerifyNoOtherCalls();
        customSitemapNodeProviderMock.Verify(x => x.GetNodes(), Times.Once);
    }

    [Fact]
    public async Task CreateSitemapAsync_WhenCacheEnabled_ShouldReturnSitemap()
    {
        // arrange
        const string SitemapResult = "<urlset></urlset>";
        var options = new SitemapMiddlewareOptions
        {
            CacheEnabled = true
        };
        var service = CreateService(options, out var hybridCacheMock, null);

        hybridCacheMock.Setup(
            x => x.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Func<It.IsAnyType, CancellationToken, ValueTask<string>>>(),
                It.IsAny<HybridCacheEntryOptions?>(),
                It.IsAny<IEnumerable<string>?>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(() => SitemapResult);

        // act
        var result = await service.CreateSitemapAsync();

        // assert
        result.Should().NotBeNullOrWhiteSpace();
        result.Should().Be(SitemapResult);
        hybridCacheMock.Verify(
            x => x.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Func<It.IsAnyType, CancellationToken, ValueTask<string>>>(),
                It.IsAny<HybridCacheEntryOptions?>(),
                It.IsAny<IEnumerable<string>?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static ApplicationSitemapService CreateService(
        SitemapMiddlewareOptions options,
        out Mock<HybridCache> hybridCacheMock,
        ICustomSitemapNodeProvider? customSitemapNodeProvider)
    {
        var sitemapServiceMock = new Mock<ISitemapService>();
        sitemapServiceMock.Setup(x => x.SerializeAsync(It.IsAny<Core.Sitemap>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult("<urlset></urlset>"));

        var controllerSitemapServiceMock = new Mock<IControllerSitemapService>();
        controllerSitemapServiceMock.Setup(x => x.CreateSitemap(It.IsAny<Type>()))
            .Returns(new HashSet<SitemapNode> {new SitemapNode("/test1")});

        var razorPagesSitemapServiceMock = new Mock<IRazorPageSitemapService>();
        razorPagesSitemapServiceMock.Setup(x => x.CreateSitemap())
            .Returns(new HashSet<SitemapNode> {new SitemapNode("/test2")});

        var controllerServiceMock = new Mock<IControllerService>();
        controllerServiceMock.Setup(x => x.GetControllersFromAssembly(It.IsAny<Type>()))
            .Returns(new List<Type> {typeof(DummyController)});

        var serviceProvider = new ServiceCollection();
        hybridCacheMock = new Mock<HybridCache>();
        serviceProvider.AddSingleton(hybridCacheMock.Object);

        if (customSitemapNodeProvider != null)
        {
            serviceProvider.AddSingleton(customSitemapNodeProvider);
        }

        return new ApplicationSitemapService(
            sitemapServiceMock.Object,
            controllerSitemapServiceMock.Object,
            Options.Create(options),
            controllerServiceMock.Object,
            razorPagesSitemapServiceMock.Object,
            serviceProvider.BuildServiceProvider(),
            new AssertLogger<ApplicationSitemapService>());
    }

    private sealed class DummyController : Controller
    {
        public IActionResult Index() => Ok();
    }
}