using System.Diagnostics.CodeAnalysis;

namespace Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Middleware.Models;

[ExcludeFromCodeCoverage]
public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}