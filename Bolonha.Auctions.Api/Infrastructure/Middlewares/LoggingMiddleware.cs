using System.Diagnostics;

namespace Bolonha.Auctions.Api.Infrastructure.Middlewares;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<LoggingMiddleware> _logger = logger;
    public async Task InvokeAsync(HttpContext context)
    {
        Stopwatch? stopwatch = null;
        if (_logger.IsEnabled(LogLevel.Debug)) stopwatch = Stopwatch.StartNew();
        _logger.LogDebug("Request: {Method} {Path}",
            context.Request.Method, context.Request.Path);

        await _next(context);

        if (_logger.IsEnabled(LogLevel.Debug)) stopwatch?.Stop();
        _logger.LogDebug("Response: {StatusCode} - Completed in {ElapsedMilliseconds} ms",
            context.Response.StatusCode, stopwatch?.ElapsedMilliseconds);
    }
}

