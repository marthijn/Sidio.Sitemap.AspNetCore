using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging.Abstractions;
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
        var service = CreateService(options, out var distributedCacheMock);

        // act
        var result = await service.CreateSitemapAsync();

        // assert
        result.Should().NotBeNullOrWhiteSpace();
        distributedCacheMock.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateSitemapAsync_WhenCacheEnabledAndCacheIsEmpty_ShouldReturnSitemap()
    {
        // arrange
        var options = new SitemapMiddlewareOptions
        {
            CacheEnabled = true
        };
        var service = CreateService(options, out var distributedCacheMock);

        distributedCacheMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult((byte[]?)null));

        // act
        var result = await service.CreateSitemapAsync();

        // assert
        result.Should().NotBeNullOrWhiteSpace();
        distributedCacheMock.Verify(
            x => x.SetAsync(
                It.IsAny<string>(),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateSitemapAsync_WhenCacheEnabledAndCacheIsNotEmpty_ShouldReturnSitemap()
    {
        // arrange
        var options = new SitemapMiddlewareOptions
        {
            CacheEnabled = true
        };
        var service = CreateService(options, out var distributedCacheMock);

        distributedCacheMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult("<urlset></urlset>"u8.ToArray())!);

        // act
        var result = await service.CreateSitemapAsync();

        // assert
        result.Should().NotBeNullOrWhiteSpace();
        distributedCacheMock.Verify(
            x => x.SetAsync(
                It.IsAny<string>(),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static ApplicationSitemapService CreateService(
        SitemapMiddlewareOptions options,
        out Mock<IDistributedCache> distributedCacheMock)
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

        distributedCacheMock = new Mock<IDistributedCache>();

        var controllerServiceMock = new Mock<IControllerService>();
        controllerServiceMock.Setup(x => x.GetControllersFromAssembly(It.IsAny<Type>()))
            .Returns(new List<Type> {typeof(DummyController)});

        return new ApplicationSitemapService(
            sitemapServiceMock.Object,
            controllerSitemapServiceMock.Object,
            distributedCacheMock.Object,
            Options.Create(options),
            controllerServiceMock.Object,
            razorPagesSitemapServiceMock.Object,
            NullLogger<ApplicationSitemapService>.Instance);
    }

    private sealed class DummyController : Controller
    {
        public IActionResult Index() => Ok();
    }
}