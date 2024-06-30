﻿namespace Sidio.Sitemap.AspNetCore.Services;

/// <summary>
/// The controller service.
/// </summary>
public interface IControllerService
{
    /// <summary>
    /// Returns a list of controllers from the entry assembly.
    /// </summary>
    /// <returns>A <see cref="IReadOnlyList{T}"/>.</returns>
    IReadOnlyList<Type> GetControllersFromEntryAssembly();
}