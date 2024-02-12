using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Sitemap.Core.Services;

namespace Sitemap.AspNetCore.Tests;

public sealed class SitemapResultTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task ExecuteResultAsync_Sitemap_ReturnsXml()
    {
        // arrange
        var sitemap = _fixture.Create<Core.Sitemap>();
        var sitemapResult = new SitemapResult(sitemap);

        var httpContext = new DefaultHttpContext { RequestServices = CreateServiceProvider(), Response = { Body = new MemoryStream() } };
        var routeData = new Microsoft.AspNetCore.Routing.RouteData();
        var actionDescriptor = new ActionDescriptor();

        var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);

        // act
        await sitemapResult.ExecuteResultAsync(actionContext);

        // assert
        httpContext.Response.Should().NotBeNull();
        httpContext.Response.ContentType.Should().Be("application/xml");

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(httpContext.Response.Body);
        var xml = await reader.ReadToEndAsync();
        xml.Should().NotBeNullOrEmpty().And.Contain("urlset");
    }

    [Fact]
    public async Task ExecuteResultAsync_SitemapIndex_ReturnsXml()
    {
        // arrange
        var sitemap = _fixture.Create<Core.SitemapIndex>();
        var sitemapResult = new SitemapResult(sitemap);

        var httpContext = new DefaultHttpContext { RequestServices = CreateServiceProvider(), Response = { Body = new MemoryStream() } };
        var routeData = new Microsoft.AspNetCore.Routing.RouteData();
        var actionDescriptor = new ActionDescriptor();

        var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);

        // act
        await sitemapResult.ExecuteResultAsync(actionContext);

        // assert
        httpContext.Response.Should().NotBeNull();
        httpContext.Response.ContentType.Should().Be("application/xml");

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(httpContext.Response.Body);
        var xml = await reader.ReadToEndAsync();
        xml.Should().NotBeNullOrEmpty().And.Contain("sitemapindex");
    }

    [Fact]
    public void ExecuteResult_Sitemap_ReturnsXml()
    {
        // arrange
        var sitemap = _fixture.Create<Core.Sitemap>();
        var sitemapResult = new SitemapResult(sitemap);

        var httpContext = new DefaultHttpContext { RequestServices = CreateServiceProvider(), Response = { Body = new MemoryStream() } };
        var routeData = new Microsoft.AspNetCore.Routing.RouteData();
        var actionDescriptor = new ActionDescriptor();

        var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);

        // act
        sitemapResult.ExecuteResult(actionContext);

        // assert
        httpContext.Response.Should().NotBeNull();
        httpContext.Response.ContentType.Should().Be("application/xml");

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(httpContext.Response.Body);
        var xml = reader.ReadToEnd();
        xml.Should().NotBeNullOrEmpty().And.Contain("urlset");
    }

    [Fact]
    public void ExecuteResult_SitemapIndex_ReturnsXml()
    {
        // arrange
        var sitemap = _fixture.Create<Core.SitemapIndex>();
        var sitemapResult = new SitemapResult(sitemap);

        var httpContext = new DefaultHttpContext { RequestServices = CreateServiceProvider(), Response = { Body = new MemoryStream() } };
        var routeData = new Microsoft.AspNetCore.Routing.RouteData();
        var actionDescriptor = new ActionDescriptor();

        var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);

        // act
        sitemapResult.ExecuteResult(actionContext);

        // assert
        httpContext.Response.Should().NotBeNull();
        httpContext.Response.ContentType.Should().Be("application/xml");

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(httpContext.Response.Body);
        var xml = reader.ReadToEnd();
        xml.Should().NotBeNullOrEmpty().And.Contain("sitemapindex");
    }

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddDefaultSitemapServices();

        return services.BuildServiceProvider();
    }
}