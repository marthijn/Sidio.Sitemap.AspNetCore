using System.Diagnostics.CodeAnalysis;
using Sidio.Sitemap.AspNetCore;
using Sidio.Sitemap.AspNetCore.Examples.WebApiApplication.Middleware;
using Sidio.Sitemap.AspNetCore.Middleware;
using Sidio.Sitemap.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddHttpContextAccessor()
    .AddDefaultSitemapServices<HttpContextBaseUrlProvider>()
    .AddSitemapMiddleware(
        options =>
        {
            options.EndpointInclusionMode = EndpointInclusionMode.OptOut;
            options.AssemblyMarker = typeof(IAssemblyMarker); // set the assembly marker, required for the integration tests
            options.IncludeApiControllers = true;
        })
    .AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSitemap();

app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program;
