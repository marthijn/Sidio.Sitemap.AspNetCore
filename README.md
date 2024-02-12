Sitemap.AspNetCore
=============
Sitemap.AspNetCore is a lightweight .NET library for generating sitemaps and a sitemap index in ASP .NET Core applications.

[![build](https://github.com/marthijn/Sitemap.AspNetCore/actions/workflows/build.yml/badge.svg)](https://github.com/marthijn/Sitemap.AspNetCore/actions/workflows/build.yml)
[![NuGet Version](https://img.shields.io/nuget/v/Sitemap.AspNetCore)](https://www.nuget.org/packages/Sitemap.AspNetCore/)

# Installation
Add [the package](https://www.nuget.org/packages/Sitemap.AspNetCore/) to your project.

# Usage
```csharp
// di setup
services.AddBaseUrlProvider<HttpContextBaseUrlProvider>();
services.AddDefaultSitemapServices();

// controller
[HttpGet]
public IActionResult Sitemap()
{
    var nodes = new List<SitemapNode> { new ("page.html") };
    var sitemap = new Sitemap(nodes);
    return SitemapResult(sitemap);
}
```