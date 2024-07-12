namespace Sidio.Sitemap.AspNetCore.Examples.MvcWebApplication.Middleware.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
