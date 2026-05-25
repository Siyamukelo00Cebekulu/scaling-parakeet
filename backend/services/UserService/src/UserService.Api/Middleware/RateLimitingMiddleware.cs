using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;

namespace UserService.Api.Middleware;

/// <summary>
/// Middleware that applies simple per-IP rate limiting for incoming HTTP requests.
/// </summary>
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private readonly int _maxRequests;
    private readonly TimeSpan _window;
    private readonly ConcurrentDictionary<string, (int Count, DateTime WindowStart)> _counters = new();

    /// <summary>
    /// Initializes a new instance of <see cref="RateLimitingMiddleware"/>.
    /// </summary>
    public RateLimitingMiddleware(RequestDelegate next, IConfiguration config, ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _maxRequests = config.GetValue<int?>("RateLimiting:MaxRequests") ?? 100;
        _window = TimeSpan.FromSeconds(config.GetValue<int?>("RateLimiting:WindowSeconds") ?? 60);
    }

    /// <summary>
    /// Executes rate limiting logic for the current HTTP context.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var now = DateTime.UtcNow;

        var entry = _counters.GetOrAdd(ip, _ => (0, now));
        if (now - entry.WindowStart > _window)
        {
            entry = (0, now);
        }

        entry = (entry.Count + 1, entry.WindowStart);
        _counters[ip] = entry;

        if (entry.Count > _maxRequests)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.Headers["Retry-After"] = _window.TotalSeconds.ToString();
            await context.Response.WriteAsync("Too many requests. Please try again later.");
            return;
        }

        await _next(context);
    }
}
