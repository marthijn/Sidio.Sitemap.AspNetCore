using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
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
        var result = service.CreateSitemap(typeof(ActionIncludeController));

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
        var result = service.CreateSitemap(typeof(ActionIncludeController));

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
        var result = service.CreateSitemap(typeof(ControllerIncludeController));

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
        var result = service.CreateSitemap(typeof(ActionExcludeController));

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
        var result = service.CreateSitemap(typeof(ActionExcludeController));

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
        var result = service.CreateSitemap(typeof(ControllerExcludeController));

        // assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void CreateSitemap_WhenUsingOptInAndAreaMvcActionDecorated_ShouldReturnNodes()
    {
        // arrange
        var service = CreateService(
            new SitemapMiddlewareOptions{EndpointInclusionMode = EndpointInclusionMode.OptIn},
            out var actionDescriptorCollectionProvider,
            address => HasArea(address, "admin") ? _fixture.Create<string>() : null);

        actionDescriptorCollectionProvider.SetupGet(x => x.ActionDescriptors).Returns(
            new ActionDescriptorCollection(
                new List<ActionDescriptor>
                {
                    new ControllerActionDescriptor
                    {
                        ControllerTypeInfo = typeof(AreaMvcController).GetTypeInfo(),
                        ActionName = nameof(AreaMvcController.Included),
                        ControllerName = "AreaMvc",
                        EndpointMetadata = [new SitemapIncludeAttribute()],
                        RouteValues = new Dictionary<string, string?> { ["area"] = "admin" }
                    }
                },
                1));

        // act
        var result = service.CreateSitemap(typeof(AreaMvcController));

        // assert
        result.Should().NotBeNullOrEmpty();
        result.Count.Should().Be(1);
    }

    [Fact]
    public void CreateSitemap_WhenUsingOptOutAndAreaMvcActionExcluded_ShouldReturnEmptyNodes()
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
                        ControllerTypeInfo = typeof(AreaMvcController).GetTypeInfo(),
                        ActionName = nameof(AreaMvcController.Excluded),
                        ControllerName = "AreaMvc",
                        EndpointMetadata = [new SitemapExcludeAttribute()],
                        RouteValues = new Dictionary<string, string?> { ["area"] = "admin" }
                    }
                },
                1));

        // act
        var result = service.CreateSitemap(typeof(AreaMvcController));

        // assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void CreateSitemap_WhenUsingOptInAndAreaApiActionDecorated_ShouldReturnNodes()
    {
        // arrange
        var service = CreateService(
            new SitemapMiddlewareOptions{EndpointInclusionMode = EndpointInclusionMode.OptIn},
            out var actionDescriptorCollectionProvider,
            address => HasArea(address, "admin") ? _fixture.Create<string>() : null);

        actionDescriptorCollectionProvider.SetupGet(x => x.ActionDescriptors).Returns(
            new ActionDescriptorCollection(
                new List<ActionDescriptor>
                {
                    new ControllerActionDescriptor
                    {
                        ControllerTypeInfo = typeof(AreaApiController).GetTypeInfo(),
                        ActionName = nameof(AreaApiController.Included),
                        ControllerName = "AreaApi",
                        EndpointMetadata = [new SitemapIncludeAttribute()],
                        RouteValues = new Dictionary<string, string?> { ["area"] = "admin" }
                    }
                },
                1));

        // act
        var result = service.CreateSitemap(typeof(AreaApiController));

        // assert
        result.Should().NotBeNullOrEmpty();
        result.Count.Should().Be(1);
    }

    [Fact]
    public void CreateSitemap_WhenUsingOptOutAndAreaApiActionExcluded_ShouldReturnEmptyNodes()
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
                        ControllerTypeInfo = typeof(AreaApiController).GetTypeInfo(),
                        ActionName = nameof(AreaApiController.Excluded),
                        ControllerName = "AreaApi",
                        EndpointMetadata = [new SitemapExcludeAttribute()],
                        RouteValues = new Dictionary<string, string?> { ["area"] = "admin" }
                    }
                },
                1));

        // act
        var result = service.CreateSitemap(typeof(AreaApiController));

        // assert
        result.Should().BeEmpty();
    }

    private static bool HasArea(RouteValuesAddress routeValuesAddress, string area) =>
        routeValuesAddress.ExplicitValues.TryGetValue("area", out var areaValue) &&
        string.Equals(areaValue?.ToString(), area, StringComparison.OrdinalIgnoreCase);

    private ControllerSitemapService CreateService(
        SitemapMiddlewareOptions options,
        out Mock<IActionDescriptorCollectionProvider> actionDescriptorCollectionProvider,
        Func<RouteValuesAddress, string?>? urlFactory = null)
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
                It.IsAny<LinkOptions?>())).Returns((
                HttpContext _,
                RouteValuesAddress routeValuesAddress,
                RouteValueDictionary _,
                RouteValueDictionary? _,
                string? _,
                HostString? _,
                PathString? _,
                FragmentString _,
                LinkOptions? _) => urlFactory?.Invoke(routeValuesAddress) ?? _fixture.Create<string>());

        actionDescriptorCollectionProvider = new Mock<IActionDescriptorCollectionProvider>();

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.SetupGet(x => x.HttpContext).Returns(new DefaultHttpContext());

        return new ControllerSitemapService(
            linkGenerator.Object,
            actionDescriptorCollectionProvider.Object,
            httpContextAccessor.Object,
            Options.Create(options),
            new AssertLogger<ControllerSitemapService>());
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

    private sealed class AreaMvcController : Controller
    {
        public IActionResult Included() => Ok();

        public IActionResult Excluded() => Ok();
    }

    [ApiController]
    private sealed class AreaApiController : ControllerBase
    {
        public IActionResult Included() => Ok();

        public IActionResult Excluded() => Ok();
    }
}