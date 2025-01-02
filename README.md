Sidio.Sitemap.AspNetCore
=============
Sidio.Sitemap.AspNetCore is a lightweight .NET library for generating sitemaps and a sitemap index in ASP .NET Core applications.

In addition to sitemap and sitemap index generation, news, images and video extensions are also supported.

[![build](https://github.com/marthijn/Sidio.Sitemap.AspNetCore/actions/workflows/build.yml/badge.svg)](https://github.com/marthijn/Sidio.Sitemap.AspNetCore/actions/workflows/build.yml)
[![NuGet Version](https://img.shields.io/nuget/v/Sidio.Sitemap.AspNetCore)](https://www.nuget.org/packages/Sidio.Sitemap.AspNetCore/)
[![Coverage Status](https://coveralls.io/repos/github/marthijn/Sidio.Sitemap.AspNetCore/badge.svg?branch=main)](https://coveralls.io/github/marthijn/Sidio.Sitemap.AspNetCore?branch=main)

# Installation
Add [the package](https://www.nuget.org/packages/Sidio.Sitemap.AspNetCore/) to your project.

# Usage
There are two ways to generate sitemaps: manually or by using middleware. When using middleware, the sitemap is generated automatically.

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
    var nodes = new List<SitemapNode> { new ("page.html"), SitemapNode.Create(Url.Action("Index")) };
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
    
    // basic usage:
    sitemapIndex.Add(new SitemapIndexNode(Url.Action("Sitemap1")));
    
    // or: this extension function fixes the null reference warning
    // on the line above:
    var addResult = sitemapIndex.TryAdd(Url.Action("Sitemap2"));
    
    // or: use the Create function
    sitemapIndex.Add(SitemapIndexNode.Create(Url.Action("Sitemap1")));
    
    return new SitemapResult(sitemapIndex);
}

[Route("sitemap-1.xml")]
public IActionResult Sitemap1()
{
    // ...
}

[Route("sitemap-2.xml")]
public IActionResult Sitemap2()
{
    // ...
}
```

### Advanced setup and extensions
See the [Sidio.Sitemap.Core package documentation](https://github.com/marthijn/Sidio.Sitemap.Core) to read more about additional properties
and sitemap extensions (i.e. news, images and videos).

## Using middleware
By using the `SitemapMiddlware` the sitemap is generated automatically using reflection. 
Currently only ASP .NET Core controllers and actions are supported. Razor pages will be supported in the future.

### Setup
In `Program.cs`, add the following:
```csharp
// di setup
builder.Services.
    .AddHttpContextAccessor()
    .AddDefaultSitemapServices<HttpContextBaseUrlProvider>()
    .AddSitemapMiddleware(
        options =>
        {
            options.EndpointInclusionMode = EndpointInclusionMode.OptIn;
            options.CacheEnabled = false; // (optional) default is false, set to true to enable caching
            options.CacheAbsoluteExpirationInMinutes = 60; // (optional) default is 60 minutes
        })

// use the middleware 
app.UseSitemap();
```

### Controller and action attributes
Decorate your controllers and/or actions with the `[SitemapInclude]` or `[SitemapExclude]` attribute.

When using `OptIn` mode, only controllers and/or actions decorated with `[SitemapInclude]` will be included in the sitemap.
```csharp
[SitemapInclude] // this action will be included in the sitemap
public IActionResult Index()
{
    return View();
}
```

When using `OptOut` mode, controllers and/or actions decorated with `[SitemapExclude]` will be excluded from the sitemap.
```csharp
[SitemapExclude] // this action will not be included in the sitemap
public IActionResult Index()
{
    return View();
}
```

#### API controllers
Indexing of API controllers is supported as well by configuring the `SitemapMiddleware`:
```csharp
builder.Services
    // ...
    .AddSitemapMiddleware(
        options =>
        {
            // ...
            options.IncludeApiControllers = true;
        })
```

### Razor pages
Similar to controllers and actions, the attributes can be used in razor pages:
```cshtml
@page
@attribute [SitemapExclude]
@model LoginModel
@{
    ViewData["Title"] = "My login page";
}
```

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