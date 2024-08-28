using System.Diagnostics.CodeAnalysis;

namespace Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Models;

[ExcludeFromCodeCoverage]
public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}