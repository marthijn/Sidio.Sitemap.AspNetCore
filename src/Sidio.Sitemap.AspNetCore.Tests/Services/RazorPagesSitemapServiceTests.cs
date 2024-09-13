using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Sidio.Sitemap.AspNetCore.Middleware;
using Sidio.Sitemap.AspNetCore.Services;

namespace Sidio.Sitemap.AspNetCore.Tests.Services;

public sealed class RazorPagesSitemapServiceTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void CreateSitemap_WhenUsingOptInAndPageDecorated_ShouldReturnNodes()
    {
        // arrange
        var service = CreateService(
            new SitemapMiddlewareOptions{EndpointInclusionMode = EndpointInclusionMode.OptIn},
            out var actionDescriptorCollectionProvider);

        actionDescriptorCollectionProvider.SetupGet(x => x.ActionDescriptors).Returns(
            new ActionDescriptorCollection(
                new List<ActionDescriptor>
                {
                    new CompiledPageActionDescriptor
                    {
                        EndpointMetadata = [new SitemapIncludeAttribute()],
                        ViewEnginePath = "/index"
                    }
                },
                1));

        // act
        var result = service.CreateSitemap();

        // assert
        result.Should().NotBeNullOrEmpty();
        result.Count.Should().Be(1);
    }

    [Fact]
    public void CreateSitemap_WhenUsingOptInAndPagesNotDecorated_ShouldReturnEmptyNodes()
    {
        // arrange
        var service = CreateService(
            new SitemapMiddlewareOptions{EndpointInclusionMode = EndpointInclusionMode.OptIn},
            out var actionDescriptorCollectionProvider);

        actionDescriptorCollectionProvider.SetupGet(x => x.ActionDescriptors).Returns(
            new ActionDescriptorCollection(
                new List<ActionDescriptor>
                {
                    new CompiledPageActionDescriptor
                    {
                        EndpointMetadata = [],
                        ViewEnginePath = "/index"
                    }
                },
                1));

        // act
        var result = service.CreateSitemap();

        // assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void CreateSitemap_WhenUsingOptOutAndPagesDecorated_ShouldReturnEmptyNodes()
    {
        // arrange
        var service = CreateService(
            new SitemapMiddlewareOptions{EndpointInclusionMode = EndpointInclusionMode.OptOut},
            out var actionDescriptorCollectionProvider);

        actionDescriptorCollectionProvider.SetupGet(x => x.ActionDescriptors).Returns(
            new ActionDescriptorCollection(
                new List<ActionDescriptor>
                {
                    new CompiledPageActionDescriptor
                    {
                        EndpointMetadata = [new SitemapExcludeAttribute()],
                        ViewEnginePath = "/index"
                    }
                },
                1));

        // act
        var result = service.CreateSitemap();

        // assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void CreateSitemap_WhenUsingOptOutAndPagesNotDecorated_ShouldReturnNodes()
    {
        // arrange
        var service = CreateService(
            new SitemapMiddlewareOptions{EndpointInclusionMode = EndpointInclusionMode.OptOut},
            out var actionDescriptorCollectionProvider);

        actionDescriptorCollectionProvider.SetupGet(x => x.ActionDescriptors).Returns(
            new ActionDescriptorCollection(
                new List<ActionDescriptor>
                {
                    new CompiledPageActionDescriptor
                    {
                        EndpointMetadata = [],
                        ViewEnginePath = "/index"
                    }
                },
                1));

        // act
        var result = service.CreateSitemap();

        // assert
        result.Should().NotBeNullOrEmpty();
        result.Count.Should().Be(1);
    }

    private RazorPagesSitemapService CreateService(
        SitemapMiddlewareOptions options,
        out Mock<IActionDescriptorCollectionProvider> actionDescriptorCollectionProvider)
    {
        var linkGenerator = new Mock<LinkGenerator>();
        linkGenerator.Setup(
            x => x.GetUriByAddress(
                It.IsAny<HttpContext>(),
                It.IsAny<RouteValuesAddress>(),
                It.IsAny<RouteValueDictionary>(),
                It.IsAny<RouteValueDictionary?>(),
                It.IsAny<string?>(),
                It.IsAny<HostString?>(),
                It.IsAny<PathString?>(),
                It.IsAny<FragmentString>(),
                It.IsAny<LinkOptions?>())).Returns(_fixture.Create<string>());

        actionDescriptorCollectionProvider = new Mock<IActionDescriptorCollectionProvider>();

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.SetupGet(x => x.HttpContext).Returns(new DefaultHttpContext());

        return new RazorPagesSitemapService(
            linkGenerator.Object,
            actionDescriptorCollectionProvider.Object,
            httpContextAccessor.Object,
            Options.Create(options),
            new AssertLogger<RazorPagesSitemapService>());
    }
}