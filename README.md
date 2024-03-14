Sidio.Sitemap.AspNetCore
=============
Sidio.Sitemap.AspNetCore is a lightweight .NET library for generating sitemaps and a sitemap index in ASP .NET Core applications.

[![build](https://github.com/marthijn/Sidio.Sitemap.AspNetCore/actions/workflows/build.yml/badge.svg)](https://github.com/marthijn/Sidio.Sitemap.AspNetCore/actions/workflows/build.yml)
[![NuGet Version](https://img.shields.io/nuget/v/Sidio.Sitemap.AspNetCore)](https://www.nuget.org/packages/Sidio.Sitemap.AspNetCore/)

# Installation
Add [the package](https://www.nuget.org/packages/Sidio.Sitemap.AspNetCore/) to your project.

# Usage
```csharp
// di setup
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

# See also
* [Sidio.Sitemap.Core package](https://github.com/marthijn/Sidio.Sitemap.Core)