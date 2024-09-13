using Microsoft.Extensions.Logging;

namespace Sidio.Sitemap.AspNetCore.Tests;

internal sealed class AssertLogger<T> : ILogger<T>
    where T : class
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);
        message.Should().NotBeNullOrWhiteSpace();
    }
}