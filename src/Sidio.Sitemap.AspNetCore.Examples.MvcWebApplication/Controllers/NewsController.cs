using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Controllers;

[ExcludeFromCodeCoverage]
public sealed class NewsController : Controller
{
    public string Article1()
    {
        return "article1";
    }
}