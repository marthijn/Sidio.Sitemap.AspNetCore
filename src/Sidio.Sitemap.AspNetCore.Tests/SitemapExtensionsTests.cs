namespace Sidio.Sitemap.AspNetCore.Tests;

public sealed class SitemapExtensionsTests
{
    [Fact]
    public void TryAdd_WhenUrlIsValid_SitemapNodeAdded()
    {
        // arrange
        var sitemap = new Core.Sitemap();
        const string Url = "/index.html";

        // act
        var result = sitemap.TryAdd(Url);

        // assert
        result.Should().BeTrue();
        sitemap.Nodes.Count.Should().Be(1);
    }

    [Fact]
    public void TryAdd_WhenUrlIsEmpty_SitemapNodeNotAdded()
    {
        // arrange
        var sitemap = new Core.Sitemap();

        // act
        var result = sitemap.TryAdd(string.Empty);

        // assert
        result.Should().BeFalse();
        sitemap.Nodes.Should().BeEmpty();
    }

    [Fact]
    public void TryAdd_WhenUrlsAreValid_SitemapNodesAdded()
    {
        // arrange
        var sitemap = new Core.Sitemap();
        const string Url = "/index.html";

        // act
        var result = sitemap.TryAdd(Url, Url, Url);

        // assert
        result.Should().Be(3);
        sitemap.Nodes.Count.Should().Be(3);
    }

    [Fact]
    public void TryAdd_WhenSomeUrlsAreEmpty_SitemapNodeAdded()
    {
        // arrange
        var sitemap = new Core.Sitemap();
        const string Url = "/index.html";

        // act
        var result = sitemap.TryAdd(Url, string.Empty, Url, " ");

        // assert
        result.Should().Be(2);
        sitemap.Nodes.Count.Should().Be(2);
    }
}