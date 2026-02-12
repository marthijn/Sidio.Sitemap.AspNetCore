using Microsoft.AspNetCore.Http;
using Sidio.Sitemap.Core;

namespace Sidio.Sitemap.AspNetCore;

/// <summary>
/// The HTTP Context base URL provider.
/// The BaseUrl property returns the base URL of the current HTTP request.
/// </summary>
/// <remarks>This function is using Request.Host, which is not considered safe when ForwardedHeaders are
/// not configured. See the readme for details.</remarks>
public sealed class HttpContextBaseUrlProvider : IBaseUrlProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpContextBaseUrlProvider"/> class.
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    public HttpContextBaseUrlProvider(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public Uri BaseUrl
    {
        get
        {
            var request = _httpContextAccessor.HttpContext?.Request ?? throw new InvalidOperationException("The HTTP context is not available.");
            return new ($"{request.Scheme}://{request.Host.Value}{request.PathBase}", UriKind.Absolute);
        }
    }
}