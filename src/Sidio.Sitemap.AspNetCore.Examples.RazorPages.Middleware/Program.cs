using Sidio.Sitemap.AspNetCore;
using Sidio.Sitemap.AspNetCore.Examples.RazorPages.Middleware;
using Sidio.Sitemap.AspNetCore.Middleware;
using Sidio.Sitemap.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor()
    .AddDefaultSitemapServices<HttpContextBaseUrlProvider>()
    .AddSitemapMiddleware(
        options =>
        {
            options.EndpointInclusionMode = EndpointInclusionMode.OptOut;
            options.AssemblyMarker = typeof(IAssemblyMarker); // set the assembly marker, required for the integration tests
        })
    .AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSitemap();

app.MapRazorPages();

app.Run();

public partial class Program;
