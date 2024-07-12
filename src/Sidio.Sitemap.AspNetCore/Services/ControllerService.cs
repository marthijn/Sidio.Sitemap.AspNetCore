using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sidio.Sitemap.AspNetCore.Services;

internal sealed class ControllerService : IControllerService
{
    private readonly ILogger<ControllerService> _logger;

    public ControllerService(ILogger<ControllerService> logger)
    {
        _logger = logger;
    }

    [ExcludeFromCodeCoverage]
    public IReadOnlyList<Type> GetControllersFromAssembly(Type? assemblyMarker = null)
    {
        var currentAssembly = assemblyMarker != null ? Assembly.GetAssembly(assemblyMarker) : Assembly.GetEntryAssembly();
        if (currentAssembly == null)
        {
            throw new InvalidOperationException("Entry assembly is null");
        }

        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace("Retrieving controllers from assembly `{Assembly}`", currentAssembly.FullName);
        }

        var types = currentAssembly.GetTypes();
        return types
            .Where(type => typeof(Controller).IsAssignableFrom(type)).ToList();
    }
}