using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Sidio.Sitemap.AspNetCore.Services;

internal sealed class ControllerService : IControllerService
{
    [ExcludeFromCodeCoverage]
    public IReadOnlyList<Type> GetControllersFromEntryAssembly()
    {
        var currentAssembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Entry assembly is null");
        var types = currentAssembly.GetTypes();
        return types
            .Where(type => typeof(Controller).IsAssignableFrom(type)).ToList();
    }
}