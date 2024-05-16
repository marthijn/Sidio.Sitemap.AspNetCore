using Microsoft.AspNetCore.Mvc.Testing;

namespace Sidio.Sitemap.AspNetCore.IntegrationTests.MvcWebApplication.Controllers;

public sealed class SitemapControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public SitemapControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SitemapIndex_ReturnsSitemapIndex()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("/sitemap.xml");

        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("sitemapindex");
    }

    [Fact]
    public async Task SitemapHome_ReturnsSitemap()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("/sitemap-home.xml");

        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("sitemap");
    }

    [Fact]
    public async Task SitemapNews_ReturnsSitemap()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("/sitemap-news.xml");

        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("news:news");
    }

    [Fact]
    public async Task SitemapImages_ReturnsSitemap()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("/sitemap-images.xml");

        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("image:image");
    }

    [Fact]
    public async Task SitemapVideos_ReturnsSitemap()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("/sitemap-videos.xml");

        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("video:video");
    }
}