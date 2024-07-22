using Sidio.Sitemap.Core;

namespace Sidio.Sitemap.AspNetCore.Tests;

public sealed class SitemapIndexExtensionsTests
{
    [Fact]
    public void TryAdd_WhenUrlIsValid_SitemapIndexNodeAdded()
    {
        // arrange
        var sitemapIndex = new SitemapIndex();
        const string Url = "/index.html";

        // act
        var result = sitemapIndex.TryAdd(Url);

        // assert
        result.Should().BeTrue();
        sitemapIndex.Nodes.Count.Should().Be(1);
    }

    [Fact]
    public void TryAdd_WhenUrlIsEmpty_SitemapIndexNodeNotAdded()
    {
        // arrange
        var sitemapIndex = new SitemapIndex();

        // act
        var result = sitemapIndex.TryAdd(string.Empty);

        // assert
        result.Should().BeFalse();
        sitemapIndex.Nodes.Should().BeEmpty();
    }
}