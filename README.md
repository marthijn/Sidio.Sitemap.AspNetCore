Sidio.Sitemap.AspNetCore
=============
Sidio.Sitemap.AspNetCore is a lightweight .NET library for generating sitemaps and a sitemap index in ASP .NET Core applications.

In addition to sitemap and sitemap index generation, news, images and video extensions are also supported.

[![build](https://github.com/marthijn/Sidio.Sitemap.AspNetCore/actions/workflows/build.yml/badge.svg)](https://github.com/marthijn/Sidio.Sitemap.AspNetCore/actions/workflows/build.yml)
[![NuGet Version](https://img.shields.io/nuget/v/Sidio.Sitemap.AspNetCore)](https://www.nuget.org/packages/Sidio.Sitemap.AspNetCore/)

# Installation
Add [the package](https://www.nuget.org/packages/Sidio.Sitemap.AspNetCore/) to your project.

# Usage
## Building sitemaps manually
### Sitemap
```csharp
// di setup
services.AddHttpContextAccessor();
services.AddDefaultSitemapServices<HttpContextBaseUrlProvider>();

// controller
[HttpGet]
public IActionResult Sitemap()
{
    var nodes = new List<SitemapNode> { new ("page.html"), new (Url.Action("Index")) };
    var sitemap = new Sitemap(nodes);
    return new SitemapResult(sitemap);
}
```

### Sitemap and sitemap index
```csharp
[Route("sitemap.xml")]
public IActionResult SitemapIndex()
{
    var sitemapIndex = new SitemapIndex();
    sitemapIndex.Add(new SitemapIndexNode(Url.Action("Sitemap1")));
    return new SitemapResult(sitemapIndex);
}

[Route("sitemap-1.xml")]
public IActionResult Sitemap1()
{
    // ...
}
```

## Using middleware
By using the `SitemapMiddlware` the sitemap is generated automatically using reflection. 
Currently only ASP .NET Core controllers and actions are supported. Razor pages will be supported in the future.

### Setup
In `Program.cs`, add the following:
```csharp
builder.Services.
    .AddHttpContextAccessor()
    .AddDefaultSitemapServices<HttpContextBaseUrlProvider>()
    .AddSitemapMiddleware(
        options =>
        {
            options.EndpointInclusionMethod = EndpointInclusionMethod.OptIn;
            options.CacheEnabled = false; // (optional) default is false, set to true to enable caching
            options.CacheAbsoluteExpirationInMinutes = 60; // (optional) default is 60 minutes
        })
// ...
app.UseSitemap();
```

### Attributes
Decorated your controllers and/or actions with the `[SitemapInclude]` or `[SitemapExclude]` attribute.

When using `OptIn` mode, only controllers and/or actions decorated with `[SitemapInclude]` will be included in the sitemap.

When using `OptOut` mode, controllers and/or actions decorated with `[SitemapExclude]` will be excluded from the sitemap.

### Caching
Configure the [`IDistributedCache`](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed) to use caching of the Sitemap.

# FAQ

* Exception: `Unable to resolve service for type 'Microsoft.AspNetCore.Http.IHttpContextAccessor' while attempting to activate 'Sidio.Sitemap.AspNetCore.HttpContextBaseUrlProvider'.` 
  * Solution: call `services.AddHttpContextAccessor();` to register the `IHttpContextAccessor`.

# See also
* [Sidio.Sitemap.Core package](https://github.com/marthijn/Sidio.Sitemap.Core)
* [Sidio.Sitemap.Blazor package](https://github.com/marthijn/Sidio.Sitemap.Blazor) for Blazor support.

# Used by
- [Drammer.com](https://drammer.com)