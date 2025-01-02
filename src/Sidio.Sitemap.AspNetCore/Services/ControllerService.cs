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
        var types = GetTypes(assemblyMarker);
        return types
            .Where(type => typeof(Controller).IsAssignableFrom(type)).ToList();
    }

    [ExcludeFromCodeCoverage]
    public IReadOnlyList<Type> GetControllerBasesFromAssembly(Type? assemblyMarker = null)
    {
        var types = GetTypes(assemblyMarker);
        return types
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToList();
    }

    private Type[] GetTypes(Type? assemblyMarker = null)
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

        return currentAssembly.GetTypes();
    }
}