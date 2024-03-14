using Microsoft.AspNetCore.Http;

namespace Sidio.Sitemap.AspNetCore.Tests;

public sealed class HttpContextBaseUrlProviderTests
{
    [Fact]
    public void BaseUrl_ReturnsBaseUrl()
    {
        // arrange
        var httpContextAccessor = new HttpContextAccessor
                                      {
                                          HttpContext = new DefaultHttpContext
                                                            {
                                                                Request =
                                                                    {
                                                                        Scheme = "https", Host = new HostString("example.com"), PathBase = "/base"
                                                                    },
                                                            },
                                      };
        var baseUrlProvider = new HttpContextBaseUrlProvider(httpContextAccessor);

        // act
        var result = baseUrlProvider.BaseUrl;

        // assert
        result.Should().NotBeNull();
        result.IsAbsoluteUri.Should().BeTrue();
        result.ToString().Should().Be("https://example.com/base");
    }
}