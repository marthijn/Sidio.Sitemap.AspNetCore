using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Sidio.Sitemap.AspNetCore.Middleware;
using Sidio.Sitemap.AspNetCore.Services;

namespace Sidio.Sitemap.AspNetCore.Tests.Services;

public sealed class ControllerSitemapServiceTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void CreateSitemap_WhenUsingOptInAndActionDecorated_ShouldReturnNodes()
    {
        // arrange
        var service = CreateService(
            new SitemapMiddlewareOptions{EndpointInclusionMode = EndpointInclusionMode.OptIn},
            out var actionDescriptorCollectionProvider);

        actionDescriptorCollectionProvider.SetupGet(x => x.ActionDescriptors).Returns(
            new ActionDescriptorCollection(
                new List<ActionDescriptor>
                {
                    new ControllerActionDescriptor
                    {
                        ControllerTypeInfo = typeof(ActionIncludeController).GetTypeInfo(),
                        ActionName = nameof(ActionIncludeController.Index),
                        ControllerName = "Dummy",
                        AttributeRouteInfo = new AttributeRouteInfo
                        {
                            Template = "index"
                        },
                        EndpointMetadata = [new SitemapIncludeAttribute()],
                    }
                },
                1));

        // act
        var result = service.CreateSitemap(typeof(ActionIncludeController)).ToList();

        // assert
        result.Should().NotBeNullOrEmpty();
        result.Count.Should().Be(1);
    }

    [Fact]
    public void CreateSitemap_WhenUsingOptInAndActionNotDecorated_ShouldReturnEmptyNodes()
    {
        // arrange
        var service = CreateService(
            new SitemapMiddlewareOptions{EndpointInclusionMode = EndpointInclusionMode.OptIn},
            out var actionDescriptorCollectionProvider);

        actionDescriptorCollectionProvider.SetupGet(x => x.ActionDescriptors).Returns(
            new ActionDescriptorCollection(
                new List<ActionDescriptor>
                {
                    new ControllerActionDescriptor
                    {
                        ControllerTypeInfo = typeof(ActionIncludeController).GetTypeInfo(),
                        ActionName = nameof(ActionIncludeController.Index),
                        ControllerName = "Dummy",
                        AttributeRouteInfo = new AttributeRouteInfo
                        {
                            Template = "index"
                        },
                        EndpointMetadata = [],
                    }
                },
                1));

        // act
        var result = service.CreateSitemap(typeof(ActionIncludeController)).ToList();

        // assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void CreateSitemap_WhenUsingOptInAndControllerDecorated_ShouldReturnNodes()
    {
        // arrange
        var service = CreateService(
            new SitemapMiddlewareOptions{EndpointInclusionMode = EndpointInclusionMode.OptIn},
            out var actionDescriptorCollectionProvider);

        actionDescriptorCollectionProvider.SetupGet(x => x.ActionDescriptors).Returns(
            new ActionDescriptorCollection(
                new List<ActionDescriptor>
                {
                    new ControllerActionDescriptor
                    {
                        ControllerTypeInfo = typeof(ControllerIncludeController).GetTypeInfo(),
                        ActionName = nameof(ControllerIncludeController.Index),
                        ControllerName = "Dummy",
                        AttributeRouteInfo = new AttributeRouteInfo
                        {
                            Template = "index"
                        },
                        EndpointMetadata = [],
                    }
                },
                1));

        // act
        var result = service.CreateSitemap(typeof(ControllerIncludeController)).ToList();

        // assert
        result.Should().NotBeNullOrEmpty();
        result.Count.Should().Be(1);
    }

    [Fact]
    public void CreateSitemap_WhenUsingOptOutAndActionDecorated_ShouldReturnEmptyNodes()
    {
        // arrange
        var service = CreateService(
            new SitemapMiddlewareOptions{EndpointInclusionMode = EndpointInclusionMode.OptOut},
            out var actionDescriptorCollectionProvider);

        actionDescriptorCollectionProvider.SetupGet(x => x.ActionDescriptors).Returns(
            new ActionDescriptorCollection(
                new List<ActionDescriptor>
                {
                    new ControllerActionDescriptor
                    {
                        ControllerTypeInfo = typeof(ActionExcludeController).GetTypeInfo(),
                        ActionName = nameof(ActionExcludeController.Index),
                        ControllerName = "Dummy",
                        AttributeRouteInfo = new AttributeRouteInfo
                        {
                            Template = "index"
                        },
                        EndpointMetadata = [new SitemapExcludeAttribute()],
                    }
                },
                1));

        // act
        var result = service.CreateSitemap(typeof(ActionExcludeController)).ToList();

        // assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void CreateSitemap_WhenUsingOptOutAndActionNotDecorated_ShouldReturnNodes()
    {
        // arrange
        var service = CreateService(
            new SitemapMiddlewareOptions{EndpointInclusionMode = EndpointInclusionMode.OptOut},
            out var actionDescriptorCollectionProvider);

        actionDescriptorCollectionProvider.SetupGet(x => x.ActionDescriptors).Returns(
            new ActionDescriptorCollection(
                new List<ActionDescriptor>
                {
                    new ControllerActionDescriptor
                    {
                        ControllerTypeInfo = typeof(ActionExcludeController).GetTypeInfo(),
                        ActionName = nameof(ActionExcludeController.Index),
                        ControllerName = "Dummy",
                        AttributeRouteInfo = new AttributeRouteInfo
                        {
                            Template = "index"
                        },
                        EndpointMetadata = [],
                    }
                },
                1));

        // act
        var result = service.CreateSitemap(typeof(ActionExcludeController)).ToList();

        // assert
        result.Should().NotBeNullOrEmpty();
        result.Count.Should().Be(1);
    }

    [Fact]
    public void CreateSitemap_WhenUsingOptOutAndControllerDecorated_ShouldReturnEmptyNodes()
    {
        // arrange
        var service = CreateService(
            new SitemapMiddlewareOptions{EndpointInclusionMode = EndpointInclusionMode.OptOut},
            out var actionDescriptorCollectionProvider);

        actionDescriptorCollectionProvider.SetupGet(x => x.ActionDescriptors).Returns(
            new ActionDescriptorCollection(
                new List<ActionDescriptor>
                {
                    new ControllerActionDescriptor
                    {
                        ControllerTypeInfo = typeof(ControllerExcludeController).GetTypeInfo(),
                        ActionName = nameof(ControllerExcludeController.Index),
                        ControllerName = "Dummy",
                        AttributeRouteInfo = new AttributeRouteInfo
                        {
                            Template = "index"
                        },
                        EndpointMetadata = [],
                    }
                },
                1));

        // act
        var result = service.CreateSitemap(typeof(ControllerExcludeController)).ToList();

        // assert
        result.Should().BeEmpty();
    }

    private ControllerSitemapService CreateService(
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

        return new ControllerSitemapService(
            linkGenerator.Object,
            actionDescriptorCollectionProvider.Object,
            httpContextAccessor.Object,
            Options.Create(options),
            NullLogger<ControllerSitemapService>.Instance);
    }

    private sealed class ActionIncludeController : Controller
    {
        public IActionResult Index() => Ok();
    }

    private sealed class ActionExcludeController : Controller
    {
        public IActionResult Index() => Ok();
    }

    [SitemapInclude]
    private sealed class ControllerIncludeController : Controller
    {
        public IActionResult Index() => Ok();
    }

    [SitemapExclude]
    private sealed class ControllerExcludeController : Controller
    {
        public IActionResult Index() => Ok();
    }
}