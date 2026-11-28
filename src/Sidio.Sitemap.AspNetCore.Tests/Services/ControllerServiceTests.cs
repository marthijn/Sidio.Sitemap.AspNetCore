using Microsoft.AspNetCore.Mvc;
using Sidio.Sitemap.AspNetCore.Services;

namespace Sidio.Sitemap.AspNetCore.Tests.Services;

public sealed class ControllerServiceTests
{
    [Fact]
    public void GetControllersFromAssembly_ShouldExcludeApiControllers()
    {
        // arrange
        var sut = new ControllerService(new AssertLogger<ControllerService>());

        // act
        var result = sut.GetControllersFromAssembly(typeof(AssemblyMarker));

        // assert
        result.Should().Contain(typeof(MvcController));
        result.Should().NotContain(typeof(ApiControllerDerivedFromController));
        result.Should().NotContain(typeof(ApiControllerDerivedFromControllerBase));
    }

    [Fact]
    public void GetApiControllerFromAssembly_ShouldOnlyIncludeApiControllers()
    {
        // arrange
        var sut = new ControllerService(new AssertLogger<ControllerService>());

        // act
        var result = sut.GetApiControllerFromAssembly(typeof(AssemblyMarker));

        // assert
        result.Should().Contain(typeof(ApiControllerDerivedFromController));
        result.Should().Contain(typeof(ApiControllerDerivedFromControllerBase));
        result.Should().NotContain(typeof(MvcController));
        result.Should().NotContain(typeof(NonApiControllerBase));
    }

    private sealed class AssemblyMarker;

    private sealed class MvcController : Controller;

    [ApiController]
    private sealed class ApiControllerDerivedFromController : Controller;

    [ApiController]
    private sealed class ApiControllerDerivedFromControllerBase : ControllerBase;

    private sealed class NonApiControllerBase : ControllerBase;
}
