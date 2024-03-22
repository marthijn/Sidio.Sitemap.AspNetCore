using Microsoft.AspNetCore.Mvc;

namespace Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Controllers;

public sealed class NewsController : Controller
{
    public string Article1()
    {
        return "article1";
    }
}