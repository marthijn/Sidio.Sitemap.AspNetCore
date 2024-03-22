using Microsoft.AspNetCore.Mvc;
using Sidio.Sitemap.Core;
using Sidio.Sitemap.Core.Extensions;

namespace Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Controllers;

public sealed class SitemapController : Controller
{
    [Route("sitemap.xml")]
    public IActionResult SitemapIndex()
    {
        var sitemapIndex = new SitemapIndex();
        sitemapIndex.Add(new SitemapIndexNode(Url.Action("SitemapHome")));
        sitemapIndex.Add(new SitemapIndexNode(Url.Action("SitemapNews")));
        sitemapIndex.Add(new SitemapIndexNode(Url.Action("SitemapImages")));
        sitemapIndex.Add(new SitemapIndexNode(Url.Action("SitemapVideos")));

        return new SitemapResult(sitemapIndex);
    }

    [Route("sitemap-home.xml")]
    public IActionResult SitemapHome()
    {
        var sitemap = new Core.Sitemap();
        sitemap.Add(new SitemapNode(Url.Action("Index", "Home")));
        sitemap.Add(new SitemapNode(Url.Action("Privacy", "Home")));

        return new SitemapResult(sitemap);
    }

    [Route("sitemap-news.xml")]
    public IActionResult SitemapNews()
    {
        var sitemap = new Core.Sitemap();
        sitemap.Add(new SitemapNewsNode(Url.Action("Article1", "News"), "Article1", "John Doe", "EN", DateTime.UtcNow));

        return new SitemapResult(sitemap);
    }

    [Route("sitemap-images.xml")]
    public IActionResult SitemapImages()
    {
        var imageLocation = new ImageLocation("non-existing-image.jpg");

        var sitemap = new Core.Sitemap();
        sitemap.Add(new SitemapImageNode(Url.Action("Index", "Home"), imageLocation));

        return new SitemapResult(sitemap);
    }

    [Route("sitemap-videos.xml")]
    public IActionResult SitemapVideos()
    {
        var video = new VideoContent("non-existing-video-thumbnail.jpg", "Video1", "Video1 description", "non-existing-video.mp4", null);

        var sitemap = new Core.Sitemap();
        sitemap.Add(new SitemapVideoNode(Url.Action("Index", "Home"), video));

        return new SitemapResult(sitemap);
    }
}