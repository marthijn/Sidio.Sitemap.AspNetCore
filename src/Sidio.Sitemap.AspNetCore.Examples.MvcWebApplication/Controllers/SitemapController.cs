using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Sidio.Sitemap.Core;
using Sidio.Sitemap.Core.Extensions;

namespace Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Controllers;

[ExcludeFromCodeCoverage]
public sealed class SitemapController : Controller
{
    [Route("sitemap.xml")]
    public IActionResult SitemapIndex()
    {
        var sitemapIndex = new SitemapIndex();
        _ = sitemapIndex.TryAdd(Url.Action("SitemapHome"));
        _ = sitemapIndex.TryAdd(Url.Action("SitemapNews"));
        _ = sitemapIndex.TryAdd(Url.Action("SitemapImages"));
        _ = sitemapIndex.TryAdd(Url.Action("SitemapVideos"));

        return new SitemapResult(sitemapIndex);
    }

    [Route("sitemap-home.xml")]
    public IActionResult SitemapHome()
    {
        var sitemap = new Core.Sitemap();

        // handle null warnings by using the TryAdd function
        _ = sitemap.TryAdd(Url.Action("Index", "Home"));
        _ = sitemap.TryAdd(Url.Action("Privacy", "Home"));

        return new SitemapResult(sitemap);
    }

    [Route("sitemap-news.xml")]
    public IActionResult SitemapNews()
    {
        var sitemap = new Core.Sitemap();

        // handle null warnings by checking the URL for null
        var url = Url.Action("Article1", "News");
        if (url != null)
        {
            sitemap.Add(new SitemapNewsNode(url, "Article1", "John Doe", "EN", DateTime.UtcNow));
        }

        return new SitemapResult(sitemap);
    }

    [Route("sitemap-images.xml")]
    public IActionResult SitemapImages()
    {
        var imageLocation = new ImageLocation("non-existing-image.jpg");

        var sitemap = new Core.Sitemap();

        // handle null warnings by using the Create function
        sitemap.Add(SitemapImageNode.Create(Url.Action("Index", "Home"), imageLocation));

        return new SitemapResult(sitemap);
    }

    [Route("sitemap-videos.xml")]
    public IActionResult SitemapVideos()
    {
        var video = new VideoContent("non-existing-video-thumbnail.jpg", "Video1", "Video1 description", "non-existing-video.mp4", null);

        var sitemap = new Core.Sitemap();
        sitemap.Add(SitemapVideoNode.Create(Url.Action("Index", "Home"), video));

        return new SitemapResult(sitemap);
    }
}