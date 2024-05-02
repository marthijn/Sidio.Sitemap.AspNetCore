Sidio.Sitemap.AspNetCore
=============
Sidio.Sitemap.AspNetCore is a lightweight .NET library for generating sitemaps and a sitemap index in ASP .NET Core applications.

In addition to sitemap and sitemap index generation, news, images and video extensions are also supported.

[![build](https://github.com/marthijn/Sidio.Sitemap.AspNetCore/actions/workflows/build.yml/badge.svg)](https://github.com/marthijn/Sidio.Sitemap.AspNetCore/actions/workflows/build.yml)
[![NuGet Version](https://img.shields.io/nuget/v/Sidio.Sitemap.AspNetCore)](https://www.nuget.org/packages/Sidio.Sitemap.AspNetCore/)

# Installation
Add [the package](https://www.nuget.org/packages/Sidio.Sitemap.AspNetCore/) to your project.

# Usage
## Sitemap
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

## Sitemap and sitemap index
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

# FAQ

* Exception: `Unable to resolve service for type 'Microsoft.AspNetCore.Http.IHttpContextAccessor' while attempting to activate 'Sidio.Sitemap.AspNetCore.HttpContextBaseUrlProvider'.` 
  * Solution: call `services.AddHttpContextAccessor();` to register the `IHttpContextAccessor`.

# See also
* [Sidio.Sitemap.Core package](https://github.com/marthijn/Sidio.Sitemap.Core)

# Used by
- [Drammer.com](https://drammer.com)