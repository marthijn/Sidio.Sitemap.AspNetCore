using Microsoft.AspNetCore.Mvc.Testing;

namespace Sidio.Sitemap.AspNetCore.Examples.WebApiApplication.Middleware.Tests.WebApiApplication.Middleware;

public sealed class SitemapMiddlewareTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public SitemapMiddlewareTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Sitemap_ReturnsSitemap()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("/sitemap.xml");

        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("WeatherForecast");
        content.Should().Contain("AlternativeGet");
    }
}