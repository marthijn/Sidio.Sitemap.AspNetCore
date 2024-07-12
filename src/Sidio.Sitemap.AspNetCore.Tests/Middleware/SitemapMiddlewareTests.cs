using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sidio.Sitemap.AspNetCore.Middleware;
using Sidio.Sitemap.AspNetCore.Services;

namespace Sidio.Sitemap.AspNetCore.Tests.Middleware;

public sealed class SitemapMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenRequestPathIsNotSitemapPath_ShouldCallNext()
    {
        // arrange
        var context = new DefaultHttpContext();
        var next = new RequestDelegate(_ => Task.CompletedTask);
        var middleware = new SitemapMiddleware(next);

        // act
        await middleware.InvokeAsync(context);

        // assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task InvokeAsync_WhenRequestPathIsSitemapPath_ShouldReturnSitemapXml()
    {
        // arrange
        var applicationSitemapServiceMock = new Mock<IApplicationSitemapService>();
        applicationSitemapServiceMock.Setup(x => x.CreateSitemapAsync(It.IsAny<CancellationToken>())).ReturnsAsync("<urlset></urlset>");

        var services = new ServiceCollection();
        services.AddScoped<IApplicationSitemapService>(_ => applicationSitemapServiceMock.Object);

        var context = new DefaultHttpContext
        {
            Request =
            {
                Path = "/sitemap.xml"
            },
            RequestServices = services.BuildServiceProvider()
        };
        var next = new RequestDelegate(_ => Task.CompletedTask);
        var middleware = new SitemapMiddleware(next);

        // act
        await middleware.InvokeAsync(context);

        // assert
        context.Response.ContentType.Should().Be("application/xml");
        context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        applicationSitemapServiceMock.VerifyAll();
    }
}