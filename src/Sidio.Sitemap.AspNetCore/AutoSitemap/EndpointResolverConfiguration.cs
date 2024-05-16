using System.Collections.ObjectModel;
using System.Reflection;

namespace Sidio.Sitemap.AspNetCore.AutoSitemap;

public sealed class EndpointResolverConfiguration
{
    public Collection<Type> Controllers { get; init; } = new ();

    public Collection<Assembly> Assemblies { get; init; } = new ();
}